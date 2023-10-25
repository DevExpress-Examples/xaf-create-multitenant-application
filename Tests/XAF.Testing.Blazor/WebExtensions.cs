using System.Diagnostics;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.AspNetCore.Components;
using XAF.Testing.RX;

namespace XAF.Testing.Blazor;

public static class WebExtensions{
    public static IObservable<TArgs> WhenCallback<TArgs>(this object source, string callbackName) 
        => new Subject<TArgs>().Use(subject => {
            source.SetPropertyValue(callbackName, EventCallback.Factory.Create<TArgs>(source, args => subject.OnNext(args)));
            return subject.AsObservable().Finally(subject.Dispose);
        });
    public static IObservable<TMemberValue> WhenCallback<TObject,TMemberValue>(this TObject source, Expression<Func<TObject, EventCallback<TMemberValue>>> callbackName) 
        => source.WhenCallback<TMemberValue>(callbackName.MemberExpressionName());
    

    public static IObservable<Process> Start(this Uri uri) 
        => new ProcessStartInfo{
            FileName = "chrome",
            Arguments = $"--user-data-dir={CreateTempProfilePath("chrome")} {uri}",
            UseShellExecute = true
        }.Start().Observe().Delay(TimeSpan.FromSeconds(1));

    private static string CreateTempProfilePath(string name){
        var path = $"{Path.GetTempPath()}\\{name}";
        if (Directory.Exists(path)){
            Directory.Delete(path,true);
        }
        Directory.CreateDirectory(path);
        return path;
    }
}