using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Services;
using OutlookInspired.Win.Services;
using XAF.Testing;
using XAF.Testing.Win.XAF;
using XAF.Testing.XAF;
using Tracing = XAF.Testing.XAF.Tracing;

namespace OutlookInspired.Win.Tests.Common{
    
    public abstract class TestBase:OutlookInspired.Tests.Common.TestBase{
        static TestBase() => AppDomain.CurrentDomain.Await(async () => await Tracing.Use());

        protected IObservable<Unit> StartTest(string user, Func<WinApplication, IObservable<Unit>> test)
            => SetupWinApplication().SelectMany(application => application
                .Use(winApplication => winApplication.StartWinTest<Unit, OutlookInspiredEFCoreDbContext>(test(winApplication)
                    .Timeout(Timeout), user,ConnectionString, LogContext)));

        protected IObservable<WinApplication> SetupWinApplication() 
            => WinApplication().Do(application => {
                application.Setup();
                application.ChangeStartupState(FormWindowState.Maximized, moveToInactiveMonitor: !RunInMainMonitor);
            });

        protected IObservable<WinApplication> WinApplication(Action<IWinApplicationBuilder> configureBuilder=null) 
            => TestContext.CurrentContext.Observe().Do(context => context.Test.FullName.WriteSection())
                .Select(_ => {
                    var application = WinApplication(ConnectionString,configureBuilder);
                    application.ConnectionString = ConnectionString;
                    application.SplashScreen = null;
                    application.DropDb();
                    return application;
                });


        private static WinApplication WinApplication(string connectionString,Action<IWinApplicationBuilder> configureBuilder=null){
            var builder = DevExpress.ExpressApp.Win.WinApplication.CreateBuilder(options => {
                options.Services.AddPlatformServices();
                options.Services.AddSingleton<IAssertFilterView, AssertFilterView>();
                options.Services.AddSingleton<IFilterViewManager, FilterViewManager>();
            }).Configure(connectionString);
            configureBuilder?.Invoke(builder);
            return builder.Build();
        }
    }
}