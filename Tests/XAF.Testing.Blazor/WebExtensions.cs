using System.Diagnostics;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.AspNetCore.Components;
using XAF.Testing.RX;

namespace XAF.Testing.Blazor;

public static class WebExtensions{
    public static IObservable<Unit> WhenApplicationStarted(this IServiceProvider serviceProvider) 
        => serviceProvider.WhenLifeTimeEvent(lifetime => lifetime.ApplicationStarted);

    private static IObservable<Unit> WhenLifeTimeEvent(this IServiceProvider serviceProvider,Func<IHostApplicationLifetime,CancellationToken> theEvent){
        var subject = new Subject<Unit>();
        theEvent(serviceProvider.GetRequiredService<IHostApplicationLifetime>()).Register(_ => subject.OnNext(), null);
        return subject.AsObservable().Take(1).Finally(() => subject.Dispose());
    }

    public static IObservable<Unit> WhenApplicationStopped(this IServiceProvider serviceProvider) 
        => serviceProvider.WhenLifeTimeEvent(lifetime => lifetime.ApplicationStopped);
    public static IObservable<Unit> WhenApplicationStopping(this IServiceProvider serviceProvider) 
        => serviceProvider.WhenLifeTimeEvent(lifetime => lifetime.ApplicationStopping);

    public static IObservable<TArgs> WhenCallback<TArgs>(this object source, string callbackName) 
        => new Subject<TArgs>().Use(subject => {
            source.SetPropertyValue(callbackName, EventCallback.Factory.Create<TArgs>(source, args => subject.OnNext(args)));
            return subject.AsObservable().Finally(subject.Dispose);
        });
    public static IObservable<TMemberValue> WhenCallback<TObject,TMemberValue>(this TObject source, Expression<Func<TObject, EventCallback<TMemberValue>>> callbackName) 
        => source.WhenCallback<TMemberValue>(callbackName.MemberExpressionName());
    

    public static IObservable<Process> Start(this Uri uri,string browser=null) 
        => new ProcessStartInfo{
            FileName = browser??"chrome",
            Arguments = $"--user-data-dir={CreateTempProfilePath(browser)} {uri}",
            UseShellExecute = true
        }.Start().Observe().Delay(TimeSpan.FromSeconds(2));

    private static string CreateTempProfilePath(string name){
        var path = $"{Path.GetTempPath()}\\{name}";
        if (!Directory.Exists(path)){
            Directory.CreateDirectory($"{Path.GetTempPath()}\\{name}");
        }

        path = $"{Path.GetTempPath()}\\{name}\\{Guid.NewGuid():N}";
        Directory.CreateDirectory(path);
        return path;
    }
}