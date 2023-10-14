using DevExpress.ExpressApp.Blazor;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing;

namespace OutlookInspired.Blazor.Tests.Common{
    public abstract class TestBase:OutlookInspired.Tests.Common.TestBase{
        public async Task<BlazorApplication> SetupBlazorApplication(Func<BlazorApplication, Task> beforeSetup = null,bool useServer=true,bool runInMainMonitor=false,bool useSecuredProvider=true){
            var connectionString = "Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=OutlookInspired";
            var application = BlazorApplication(useServer, useSecuredProvider, connectionString);
            application.DeleteModelDiffs(connectionString,nameof(OutlookInspiredEFCoreDbContext.ModelDifferences),nameof(OutlookInspiredEFCoreDbContext.ModelDifferenceAspects));
              
            if (beforeSetup != null){
                await beforeSetup(application);
            }
            application.Setup();
            // application.ChangeStartupState(FormWindowState.Maximized,moveToInactiveMonitor:!runInMainMonitor);
            return application;
        }

        private static BlazorApplication BlazorApplication(bool useServer, bool useSecuredProvider, string connectionString){
            throw new NotImplementedException();
//             var builder = DevExpress.ExpressApp.Win.WinApplication.CreateBuilder(options => options.Services.AddPlatformServices());
// #if TEST
//             var application = builder.BuildApplication(useServer?null:connectionString,useSecuredProvider,"http://localhost:5000/");
// #else
//             var application = builder.BuildApplication(useServer ? null : connectionString, useSecuredProvider);
//             
// #endif
//             return application;
        }

    }
}