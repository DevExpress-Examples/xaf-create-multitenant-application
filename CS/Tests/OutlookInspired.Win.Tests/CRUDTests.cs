using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Win.Tests.Common.TestBase;

namespace OutlookInspired.Win.Tests{
    [Apartment(ApartmentState.STA)]
    [Order(50)]
    public class CRUDTests:TestBase{
        
        [RetryTestCaseSource(nameof(EmployeeVariants),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Employee(string user, string view, string viewVariant) 
            => await StartTest(user, application => application.AssertEmployeeListView(view, viewVariant));

        [RetryTestCaseSource(nameof(CustomerVariants),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Customer(string user, string view, string viewVariant) 
            => await StartTest(user, application => application.AssertCustomerListView(view, viewVariant));

        [RetryTestCaseSource(nameof(ProductVariants),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Product(string user, string view, string viewVariant) 
            => await StartTest(user, application => application.AssertProductListView(view, viewVariant));

        [RetryTestCaseSource(nameof(OrderVariants),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Order(string user, string view, string viewVariant) 
            => await StartTest(user, application => application.AssertOrderListView(view, viewVariant));
    }
}