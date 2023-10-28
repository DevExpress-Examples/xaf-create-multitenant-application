using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Blazor.Tests.Common.TestBase;

namespace OutlookInspired.Blazor.Tests{
    [Order(10)]
    public class ReportsTests : TestBase{
        
        [RetryTestCaseSource(nameof(CustomerVariants),MaxTries=MaxTries)]
        // [Category(Tests)]
        public async Task Customer(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertCustomerReports(view, viewVariant));
        }
        [RetryTestCaseSource(nameof(ProductVariants),MaxTries=MaxTries)]
        // [Category(Tests)]
        public async Task Product(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertProductReports(view, viewVariant));
        }
        [RetryTestCaseSource(nameof(OrderVariants),MaxTries=MaxTries)]
        // [Category(Tests)]
        public async Task Order(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertOrderReports(view, viewVariant));
        }
    }
}