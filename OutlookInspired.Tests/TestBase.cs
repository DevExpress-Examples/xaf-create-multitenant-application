using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Win;
using OutlookInspired.Win.Extensions;
using XAF.Testing.XAF;
using ApplicationBuilder = OutlookInspired.Win.ApplicationBuilder;

namespace OutlookInspired.Tests.ImportData{
    public abstract class TestBase{
        public async Task<WinApplication> SetupWinApplication(Func<WinApplication, Task> beforeSetup = null,bool useServer=true,bool runInMainMonitor=false,bool useSecuredProvider=true){
            var connectionString = "Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=OutlookInspired";
            var application = ApplicationBuilder.BuildApplication(useServer?null:connectionString,useSecuredProvider);
            application.DeleteModelDiffs(connectionString,nameof(OutlookInspiredEFCoreDbContext.ModelDifferences),nameof(OutlookInspiredEFCoreDbContext.ModelDifferenceAspects));
            application.SplashScreen = null;  
            if (beforeSetup != null){
                await beforeSetup(application);
            }
            application.Setup();
            application.ChangeStartupState(FormWindowState.Maximized,moveToInactiveMonitor:!runInMainMonitor);
            return application;
        }

    }
}