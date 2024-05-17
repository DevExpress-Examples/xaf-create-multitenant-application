using System.Reactive.Linq;
using DevExpress.ExpressApp.MultiTenancy;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.Persistent.BaseImpl.EF.MultiTenancy;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Win.Services;
using OutlookInspired.Win.Tests.Common;
using Shouldly;
using XAF.Testing;
using XAF.Testing.XAF;

namespace OutlookInspired.Win.Tests.Import{
    
    public class ImportData:TestBase{
        [Category(nameof(ImportData))]
        [Test]
        public async Task Test(){
            var builder = DevExpress.ExpressApp.Win.WinApplication.CreateBuilder();
            builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
            builder.AddModules();
            builder.AddObjectSpaceProviders();
            builder.Services.AddScoped<IConnectionStringProvider, ImportConnectionStringProvider>();
            using var application = builder.Build();
            await application.GetRequiredService<OutlookInspiredEFCoreDbContext>().Database.EnsureDeletedAsync();
            application.Setup();
            await using var sqlConnection = new SqlConnection(application.GetRequiredService<IConnectionStringProvider>().GetConnectionString());
            sqlConnection.Open();
            await using var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "ALTER DATABASE [OutlookInspired_Service] SET COMPATIBILITY_LEVEL = 100";
            sqlCommand.ExecuteNonQuery();
            using var objectSpace = application.ObjectSpaceProvider.CreateObjectSpace();
            
            
            objectSpace.GetObjectsQuery<Tenant>().Count().ShouldBe(0);
            await objectSpace.ImportFromSqlLite();
            objectSpace.CommitChanges();
            objectSpace.Count<Crest>().ShouldBe(20);
            objectSpace.Count<State>().ShouldBe(51);
            objectSpace.Count<Customer>().ShouldBe(20);
            objectSpace.Count<Picture>().ShouldBe(112);
            objectSpace.Count<Probation>().ShouldBe(4);
            objectSpace.Count<CustomerStore>().ShouldBe(200);
            objectSpace.Count<Employee>().ShouldBe(51);
            objectSpace.Count<ProductImage>().ShouldBe(76);
            objectSpace.Count<ProductCatalog>().ShouldBe(19);
            objectSpace.Count<Evaluation>().ShouldBe(127);
            objectSpace.Count<Product>().ShouldBe(19);
            objectSpace.Count<CustomerCommunication>().ShouldBe(1);
            objectSpace.Count<EmployeeTask>().ShouldBe(220);
            objectSpace.Count<TaskAttachedFile>().ShouldBe(84);
            objectSpace.Count<CustomerEmployee>().ShouldBe(600);
            objectSpace.Count<Order>().ShouldBe(4720);
            objectSpace.Count<OrderItem>().ShouldBe(14440);
            objectSpace.Count<Quote>().ShouldBe(8788);
            objectSpace.Count<QuoteItem>().ShouldBe(26859);
            1.Range(100).Do(user => objectSpace.CreateObject<ApplicationUser>().UserName = $"New user {user}")
                .Finally(objectSpace.CommitChanges).Enumerate();
              
            objectSpace.GenerateOrders();
        }

        class ImportConnectionStringProvider:IConnectionStringProvider{
            public string GetConnectionString() => $"{ConnectionString}{ServiceDbName}";
        }    
    }
}