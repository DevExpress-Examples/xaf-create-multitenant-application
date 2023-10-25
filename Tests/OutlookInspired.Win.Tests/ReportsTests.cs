using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Win.Tests.Common.TestBase;

namespace OutlookInspired.Win.Tests{
    [Apartment(ApartmentState.STA)]
    public class ReportsTests : TestBase{
        [RetryTestCaseSource(nameof(CustomerVariants),MaxTries=MaxTries)]
        public async Task Customer(string user,string view,string viewVariant){
            await StartWinTest(user, application => application.AssertCustomerReports(view, viewVariant));
        }
        [RetryTestCaseSource(nameof(ProductVariants),MaxTries=MaxTries)]
        public async Task Product(string user,string view,string viewVariant){
            await StartWinTest(user, application => application.AssertProductReports(view, viewVariant));
        }
        [RetryTestCaseSource(nameof(OrderVariants),MaxTries=MaxTries)]
        public async Task Order(string user,string view,string viewVariant){
            await StartWinTest(user, application => application.AssertOrderReports(view, viewVariant));
        }
    }
}