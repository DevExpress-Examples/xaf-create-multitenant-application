using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Blazor;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing;
using XAF.Testing.Blazor.XAF;

namespace OutlookInspired.Blazor.Tests.Common{
    public abstract class TestBase:OutlookInspired.Tests.Common.TestBase{
        
        public IObservable<Unit> StartBlazorTest(string user, Func<BlazorApplication, IObservable<Unit>> test,
            string browser, WindowPosition inactiveMonitorLocation = WindowPosition.None) 
            => Host.CreateDefaultBuilder().Run("http://localhost:5000", "../../../../../OutlookInspired.Blazor.Server",
                (builder, whenCompleted) => builder.UseStartup(context 
                    => context.Use<Startup,OutlookInspiredEFCoreDbContext>(test, user, whenCompleted,browser, inactiveMonitorLocation)), browser)
                .Timeout(Timeout);
        
        public IObservable<Unit> StartTest(string user, Func<BlazorApplication, IObservable<Unit>> test) 
            => StartBlazorTest(user, test,
                    browser:"chrome",inactiveMonitorLocation:WindowPosition.FullScreen)
                // .Log(LogContext, inactiveMonitorLocation:WindowPosition.BottomRight,alwaysOnTop:true)
            ;
    }
}