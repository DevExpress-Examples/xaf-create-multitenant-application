using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using Microsoft.EntityFrameworkCore;
using OutlookInspired.Win;
using OutlookInspired.Win.Extensions;

namespace Tests{
    public abstract class TestBase{
        protected async Task<WinApplication> SetupWinApplication(Func<WinApplication, Task>? beforeSetup=null){
            var builder = WinApplication.CreateBuilder();
            builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
            builder.AddModules();
            builder.AddObjectSpaceProviders(options => options.UseSqlServer("Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=OutlookInspired"));
            var application = builder.Build();
            if (beforeSetup != null){
                await beforeSetup(application);
            }
            application.Setup();
            return application;
        }

    }
}