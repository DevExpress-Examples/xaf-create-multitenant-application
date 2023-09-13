using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using Microsoft.EntityFrameworkCore;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Win;
using OutlookInspired.Win.Extensions;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData{
    public abstract class TestBase{
        public async Task<WinApplication> SetupWinApplication(Func<WinApplication, Task> beforeSetup = null,bool useServer=true,bool runInMainMonitor=false){
            var builder = WinApplication.CreateBuilder();
            builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
            builder.AddModules();
            var connectionString = "Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=OutlookInspired";
            if (!useServer){
                builder.AddObjectSpaceProviders(options => {
                    options.UseSqlServer(connectionString);
                    return true;
                });
            }
            else{
                builder.AddObjectSpaceProviders();
                builder.AddSecurity();
                builder.AddBuildStep(application => application.DatabaseUpdateMode = DatabaseUpdateMode.Never);
            }
            var application = builder.Build();
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