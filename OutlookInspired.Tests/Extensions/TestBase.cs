using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using Microsoft.EntityFrameworkCore;
using OutlookInspired.Win;
using OutlookInspired.Win.Extensions;

namespace OutlookInspired.Tests.ImportData.Extensions{
    public abstract class TestBase{
        protected static TimeSpan Timeout = UtilityExtensions.TimeoutInterval;
        public async Task<WinApplication> SetupWinApplication(Func<WinApplication, Task> beforeSetup=null){
            var builder = WinApplication.CreateBuilder();
            builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
            builder.AddModules();
            builder.AddObjectSpaceProviders(options => options.UseSqlServer("Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=OutlookInspired"));
            var application = builder.Build();
            application.SplashScreen = null;  
            if (beforeSetup != null){
                await beforeSetup(application);
            }
            application.Setup();
            application.DeleteModelDiffs();
            application.ChangeStartupState(FormWindowState.Maximized);
            return application;
        }

        
    }
}