using DevExpress.ExpressApp.Win;
using Microsoft.Extensions.DependencyInjection;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Win.Extensions;
using XAF.Testing.Win.XAF;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Common{
    public abstract class TestBase{
        public async Task<WinApplication> SetupWinApplication(Func<WinApplication, Task> beforeSetup = null,bool useServer=true,bool runInMainMonitor=false,bool useSecuredProvider=true){
            var connectionString = "Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=OutlookInspired";
            var application = XafApplication(useServer, useSecuredProvider, connectionString);
            application.DeleteModelDiffs(connectionString,nameof(OutlookInspiredEFCoreDbContext.ModelDifferences),nameof(OutlookInspiredEFCoreDbContext.ModelDifferenceAspects));
            application.SplashScreen = null;  
            if (beforeSetup != null){
                await beforeSetup(application);
            }
            application.Setup();
            application.ChangeStartupState(FormWindowState.Maximized,moveToInactiveMonitor:!runInMainMonitor);
            return application;
        }

        private static WinApplication XafApplication(bool useServer, bool useSecuredProvider, string connectionString){
            var builder = WinApplication.CreateBuilder(options => {
                options.Services.AddSingleton<ITabControlObserver, TabControlObserver>();
                options.Services.AddSingleton<IDashboardColumnViewObjectSelector, DashboardColumnViewObjectSelector>();
                options.Services.AddSingleton<IFrameObjectObserver, FrameObjectObserver>();
                options.Services.AddSingleton<INewObjectController, NewObjectController>();
                options.Services.AddSingleton<INewRowAdder, NewRowAdder>();
                options.Services.AddSingleton<IReportAsserter, ReportAsserter>();
                options.Services.AddSingleton<ISelectedObjectProcessor, SelectedObjectProcessor>();
                options.Services.AddSingleton(typeof(IObjectSelector<>),typeof(ObjectSelector<>));
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