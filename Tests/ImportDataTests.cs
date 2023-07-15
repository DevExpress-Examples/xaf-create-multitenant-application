using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.DatabaseUpdate;
using OutlookInspired.Win;
using OutlookInspired.Win.Extensions;
using Shouldly;

namespace Tests{
    public class ImportDataTests{
        [Test]
        public async Task ImportFrom(){
            var builder = WinApplication.CreateBuilder();
            builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
            builder.AddModules();
            builder.AddObjectSpaceProviders(options => options.UseInMemoryDatabase("Test"));
            using var application = builder.Build();
            application.Setup();
            var objectSpace = application.CreateObjectSpace(typeof(MigrationBaseObject));
            await objectSpace.ImportFromSqlLite();
            // objectSpace.GetObjectsQuery<CustomerStore>().Count().ShouldBe(0);
            
            // await using var sqlServerContext = new OutlookInspiredEFCoreDbContext(
            //     new DbContextOptionsBuilder<OutlookInspiredEFCoreDbContext>()
            //         .UseInMemoryDatabase("InMemory").UseChangeTrackingProxies().UseObjectSpaceLinkProxies().Options);
            //
            // await sqlServerContext.ImportFromSqlLite();
        }

        
    }
}