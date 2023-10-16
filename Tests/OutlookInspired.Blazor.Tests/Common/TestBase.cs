using System.Reactive;
using DevExpress.ExpressApp.Blazor;
using XAF.Testing.Blazor.XAF;

namespace OutlookInspired.Blazor.Tests.Common{
    public abstract class TestBase:OutlookInspired.Tests.Common.TestBase{
        public IObservable<Unit> StartBlazorApp(string user,Func<BlazorApplication, IObservable<Unit>> test,string browser,bool moveToInactiveWindow) 
            => Host.CreateDefaultBuilder().Run(new Uri("http://localhost:5010"),"../../../../../OutlookInspired.Blazor.Server",
                (builder, whenCompleted) => builder.UseStartup(context => context.Use<Startup>(test,user, whenCompleted,moveToInactiveWindow)),browser);

    }
}