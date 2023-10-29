using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Blazor;
using NUnit.Framework;
using OutlookInspired.Blazor.Server;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Services;
using XAF.Testing;
using XAF.Testing.Blazor.XAF;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Blazor.Tests.Common{
    public abstract class TestBase:OutlookInspired.Tests.Common.TestBase{
        
        protected IObservable<Unit> StartTest(string user,Func<BlazorApplication,IObservable<Unit>> test) 
            => Host.CreateDefaultBuilder().Observe().Do(_ => TestContext.CurrentContext.Test.FullName.WriteSection())
                .StartTest<Startup,OutlookInspiredEFCoreDbContext>("http://localhost:5000", "../../../../../OutlookInspired.Blazor.Server",user, test,
                    Configure,Environment.GetEnvironmentVariable("XAFTESTBrowser") ,WindowPosition.FullScreen,LogContext.None,WindowPosition.BottomRight)
                .Timeout(Timeout);

        private void Configure(IServiceCollection collection){
            collection.AddScoped<IPdfViewerAssertion,PdfViewerAssertion>();
            collection.AddScoped<IAssertMapControl,AssertMapControl>();
            collection.AddScoped<IAssertFilterView,AssertAssertFilterView>();
            collection.AddScoped<IFilterViewManager,FilterViewManager>();
            collection.AddScoped<IDashboardColumnViewObjectSelector,DashboardColumnViewObjectSelector>();
            collection.AddScoped<IUserControlProvider, UserControlProvider>();
            collection.AddScoped<IUserControlProperties, UserControlProperties>();
        }
        
        public IObservable<Unit> StartBlazorTest(string user, Func<BlazorApplication, IObservable<Unit>> test,
            string browser, WindowPosition inactiveMonitorLocation = WindowPosition.None){
            throw new NotImplementedException();
            // return Host.CreateDefaultBuilder().Run("http://localhost:5000",
            //         "../../../../../OutlookInspired.Blazor.Server",
            //         (builder, whenCompleted) => builder.UseStartup(context
            //             => context.Use<Startup, OutlookInspiredEFCoreDbContext>(test, user, whenCompleted, browser,
            //                 inactiveMonitorLocation)), browser)
            //     .Timeout(Timeout);
        }

        // public IObservable<Unit> StartTest(string user, Func<BlazorApplication, IObservable<Unit>> test)
        //
        //     => StartBlazorTest(user, test,
        //             browser:Environment.GetEnvironmentVariable("XAFTESTBrowser"),inactiveMonitorLocation:WindowPosition.FullScreen)
        //         // .Log(LogContext, inactiveMonitorLocation:WindowPosition.BottomRight,alwaysOnTop:true)
        //     ;
    }
}