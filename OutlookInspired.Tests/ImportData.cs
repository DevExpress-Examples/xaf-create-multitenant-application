using System.Reactive.Linq;
using DevExpress.ExpressApp.Testing.DevExpress.ExpressApp;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.ImportData.Extensions;
using Shouldly;
using ObjectSpaceExtensions = OutlookInspired.Module.Services.ObjectSpaceExtensions;

namespace OutlookInspired.Tests.ImportData{
    public class ImportData:TestBase{
        [Test][Description("Deletes the sqlsever database if exists and imports from $(SQLiteFilePath)")]
        // [Ignore("")]
        public async Task Test(){
            
            using var application = await SetupWinApplication(application => 
                application.ServiceProvider.GetRequiredService<OutlookInspiredEFCoreDbContext>().Database.EnsureDeletedAsync());
            
            var objectSpace = application.ObjectSpaceProvider.CreateObjectSpace();
            await objectSpace.ImportFromSqlLite();
            ObjectSpaceExtensions.Count<Crest>(objectSpace).ShouldBe(20);
            ObjectSpaceExtensions.Count<State>(objectSpace).ShouldBe(51);
            ObjectSpaceExtensions.Count<Customer>(objectSpace).ShouldBe(20);
            ObjectSpaceExtensions.Count<Picture>(objectSpace).ShouldBe(112);
            ObjectSpaceExtensions.Count<Probation>(objectSpace).ShouldBe(4);
            ObjectSpaceExtensions.Count<CustomerStore>(objectSpace).ShouldBe(200);
            ObjectSpaceExtensions.Count<Employee>(objectSpace).ShouldBe(51);
            ObjectSpaceExtensions.Count<ProductImage>(objectSpace).ShouldBe(76);
            ObjectSpaceExtensions.Count<ProductCatalog>(objectSpace).ShouldBe(19);
            ObjectSpaceExtensions.Count<Evaluation>(objectSpace).ShouldBe(127);
            ObjectSpaceExtensions.Count<Product>(objectSpace).ShouldBe(19);
            ObjectSpaceExtensions.Count<CustomerCommunication>(objectSpace).ShouldBe(1);
            ObjectSpaceExtensions.Count<EmployeeTask>(objectSpace).ShouldBe(220);
            ObjectSpaceExtensions.Count<TaskAttachedFile>(objectSpace).ShouldBe(84);
            ObjectSpaceExtensions.Count<CustomerEmployee>(objectSpace).ShouldBe(600);
            ObjectSpaceExtensions.Count<Order>(objectSpace).ShouldBe(4720);
            ObjectSpaceExtensions.Count<OrderItem>(objectSpace).ShouldBe(14440);
            ObjectSpaceExtensions.Count<Quote>(objectSpace).ShouldBe(8788);
            ObjectSpaceExtensions.Count<QuoteItem>(objectSpace).ShouldBe(26859);
            
            
            
            
            
            
        }

    }
}