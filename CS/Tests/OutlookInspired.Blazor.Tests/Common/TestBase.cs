using System.Net.NetworkInformation;
using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.EFCore;
using NUnit.Framework;
using OutlookInspired.Blazor.Server;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Services;
using XAF.Testing;
using XAF.Testing.Blazor.XAF;
using XAF.Testing.XAF;
using IUserControlObjects = XAF.Testing.XAF.IUserControlObjects;

namespace OutlookInspired.Blazor.Tests.Common{
    public abstract class TestBase:OutlookInspired.Tests.Common.TestBase{
        // static TestBase() => DatabaseSchemaStateCache.Enabled = false;
        protected WindowPosition BrowserPosition=>WindowPosition.FullScreen;
        protected WindowPosition LoggerPosition=>WindowPosition.BottomRight|WindowPosition.Small;
        protected IObservable<Unit> StartTest(string user,Func<BlazorApplication,IObservable<Unit>> test) 
            => Host.CreateDefaultBuilder().Observe()
                .Do(_ => TestContext.CurrentContext.Test.FullName.WriteSection())
                .StartTest<Startup, OutlookInspiredEFCoreDbContext>(
                    $"http://localhost:{IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().GetRandomAvailablePort()}",
                    "../../../../../OutlookInspired.Blazor.Server", user, test, Configure, Environment.GetEnvironmentVariable("XAFTESTBrowser"),
                    BrowserPosition, LogContext, LoggerPosition)
                .Timeout(Timeout);

        private void Configure(IServiceCollection collection){
            collection.AddScoped<IPdfViewerAssertion,PdfViewerAssertion>();
            collection.AddScoped<IUserControlProcessSelectedObject,UserControlProcessSelectedObject>();
            collection.AddScoped<IAssertMapControl,AssertMapControl>();
            collection.AddScoped<IFilterViewManager,FilterViewManager>();
            collection.AddScoped<IDashboardColumnViewObjectSelector,DashboardColumnViewObjectSelector>();
            collection.AddScoped<IUserControlProvider, UserControlProvider>();
            collection.AddScoped<IUserControlObjects, UserControlProperties>();
        }
    }
}