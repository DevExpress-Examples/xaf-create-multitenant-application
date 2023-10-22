using System.Reactive;
using DevExpress.ExpressApp.Blazor;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing;
using XAF.Testing.Blazor.XAF;

namespace OutlookInspired.Blazor.Tests.Common{
    public abstract class TestBase:OutlookInspired.Tests.Common.TestBase{
        public IObservable<Unit> StartBlazorTest(string user, Func<BlazorApplication, IObservable<Unit>> test,
            string browser, WindowPosition inactiveMonitorLocation = WindowPosition.None) 
            => Host.CreateDefaultBuilder().Run("http://localhost:5010", "../../../../../OutlookInspired.Blazor.Server",
                (builder, whenCompleted) => builder.UseStartup(context 
                    => context.Use<Startup,OutlookInspiredEFCoreDbContext>(test, user, whenCompleted, inactiveMonitorLocation)), browser);
    }
}