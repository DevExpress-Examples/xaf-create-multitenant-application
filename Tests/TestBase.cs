using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Win;
using OutlookInspired.Win.Extensions;

namespace Tests{
    public abstract class TestBase{
        protected async Task<WinApplication> WinApplication(){
            var testName = "Test";
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{testName}.db");
            if (File.Exists(dbPath)){
                File.Delete(dbPath);
            }
            var builder = DevExpress.ExpressApp.Win.WinApplication.CreateBuilder();
            builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
            builder.AddModules();
            // builder.AddObjectSpaceProviders(options => options.UseSqlite($"Data Source={testName}.db"));
            builder.AddObjectSpaceProviders(options => options.UseSqlServer("Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Test"));
            var application = builder.Build();
            var dbContext = application.ServiceProvider.GetRequiredService<OutlookInspiredEFCoreDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            // await dbContext.SaveChangesAsync();
            application.Setup();
            return application;
        }

    }
}