using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Import;
using OutlookInspired.Win.Tests.Common;
using Shouldly;
using XAF.Testing.XAF;

namespace OutlookInspired.Win.Tests.Import{
    
    public class ImportData:TestBase{
        [Category(nameof(ImportData))]
        [Test]
        public async Task Test(){
            
            using var application = await SetupWinApplication(application => {
                application.Security = null;
                return application.GetRequiredService<OutlookInspiredEFCoreDbContext>().Database.EnsureDeletedAsync();
            },useServer:false,useSecuredProvider:false);
            
            
            using var objectSpace = application.ObjectSpaceProvider.CreateObjectSpace();
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
            
            // objectSpace.GenerateOrders();
        }

        
    }
}