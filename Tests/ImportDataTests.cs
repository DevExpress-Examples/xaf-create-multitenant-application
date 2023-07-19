using NUnit.Framework;
using OutlookInspired.Module;
using OutlookInspired.Module.BusinessObjects;
using Shouldly;

namespace Tests{
    public class ImportDataTests:TestBase{
        [Test]
        public async Task ImportFrom(){
            using var application = await WinApplication();
            var objectSpace = application.NewObjectSpace();

            objectSpace.Count<Crest>().ShouldBe(20);
            objectSpace.Count<State>().ShouldBe(51);
            objectSpace.Count<Customer>().ShouldBe(20);
            objectSpace.Count<CustomerStore>().ShouldBe(200);
            objectSpace.Count<Picture>().ShouldBe(112);
            objectSpace.Count<Probation>().ShouldBe(4);
            objectSpace.Count<Employee>().ShouldBe(51);
            objectSpace.Count<Evaluation>().ShouldBe(127);
            objectSpace.Count<CustomerEmployee>().ShouldBe(600);
            objectSpace.Count<CustomerCommunication>().ShouldBe(1);
            objectSpace.Count<EmployeeTask>().ShouldBe(220);
            objectSpace.Count<TaskAttachedFile>().ShouldBe(84);
            objectSpace.Count<Product>().ShouldBe(19);
            objectSpace.Count<ProductImage>().ShouldBe(76);
            objectSpace.Count<ProductCatalog>().ShouldBe(19);
            objectSpace.Count<Order>().ShouldBe(4720);
            objectSpace.Count<OrderItem>().ShouldBe(14440);
            objectSpace.Count<Quote>().ShouldBe(8788);
            objectSpace.Count<QuoteItem>().ShouldBe(26859);
        }

    }
}