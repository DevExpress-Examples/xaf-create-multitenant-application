using DevExpress.ExpressApp.Win;
using OutlookInspired.Win.Extensions;
using XAF.Testing.Win.XAF;

namespace OutlookInspired.Win.Tests.Common{
    public abstract class TestBase:OutlookInspired.Tests.Common.TestBase{
        public async Task<WinApplication> SetupWinApplication(Func<WinApplication, Task> beforeSetup = null,bool useServer=true,bool runInMainMonitor=false,bool useSecuredProvider=true){
            var application = WinApplication(useServer, useSecuredProvider, ConnectionString);
            DeleteModelDiffs(application);
            application.SplashScreen = null;  
            if (beforeSetup != null){
                await beforeSetup(application);
            }
            application.Setup();
            application.ChangeStartupState(FormWindowState.Maximized,moveToInactiveMonitor:!runInMainMonitor);
            return application;
        }

        private static WinApplication WinApplication(bool useServer, bool useSecuredProvider, string connectionString){
            var builder = DevExpress.ExpressApp.Win.WinApplication.CreateBuilder(options => options.Services.AddPlatformServices());
#if TEST
            var application = builder.BuildApplication(useServer?null:connectionString,useSecuredProvider,"http://localhost:5000/");
#else
            var application = builder.BuildApplication(useServer ? null : connectionString, useSecuredProvider);
            
#endif
            return application;
        }

    }
}