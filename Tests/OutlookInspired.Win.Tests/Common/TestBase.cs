using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Win;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Services;
using OutlookInspired.Win.Services;
using XAF.Testing;
using XAF.Testing.RX;
using XAF.Testing.Win.XAF;
using Tracing = XAF.Testing.XAF.Tracing;

namespace OutlookInspired.Win.Tests.Common{
    
    public abstract class TestBase:OutlookInspired.Tests.Common.TestBase{
        static TestBase() => AppDomain.CurrentDomain.Await(async () => await Tracing.Use());
        public IObservable<Unit> StartTest(string user, Func<WinApplication, IObservable<Unit>> test)
            => SetupWinApplication().SelectMany(application => application.Use(winApplication =>
                winApplication.StartWinTest(test(winApplication)
                    .Timeout(Timeout), user, LogContext)));
        
        public IObservable<WinApplication> SetupWinApplication() 
            => WinApplication().Do(application => {
                TestContext.CurrentContext.Test.FullName.WriteSection();
                application.Setup();
                application.ChangeStartupState(FormWindowState.Maximized, moveToInactiveMonitor: !RunInMainMonitor);
            });

        public IObservable<WinApplication> WinApplication() 
            => Observable.Defer(() => {
                var application = WinApplication(UseServer, UseSecuredProvider, ConnectionString);
                application.ConnectionString = ConnectionString;
                application.DeleteModelDiffs<OutlookInspiredEFCoreDbContext>();
                application.SplashScreen = null;
                return application.Observe();
            });

        
        protected virtual bool UseSecuredProvider => true;
        protected virtual bool UseServer => false;
        
        
        private static WinApplication WinApplication(bool useServer, bool useSecuredProvider, string connectionString){
            var builder = DevExpress.ExpressApp.Win.WinApplication.CreateBuilder(options => {
                options.Services.AddPlatformServices();
                options.Services.AddSingleton<IAssertFilterView, AssertFilterView>();
                options.Services.AddSingleton<IFilterViewManager, FilterViewManager>();
            });
#if TEST
            var application = builder.BuildApplication(useServer?null:connectionString,useSecuredProvider,"http://localhost:5000/");
#else
            var application = builder.BuildApplication(useServer ? null : connectionString, useSecuredProvider);
            
#endif
            return application;
        }

    }
}