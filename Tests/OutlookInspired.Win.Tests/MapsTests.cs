using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Win.Tests.Common.TestBase;

namespace OutlookInspired.Win.Tests{
    [Apartment(ApartmentState.STA)]
    public class MapsTests:TestBase{
        [RetryTestCaseSource(nameof(EmployeeVariants),MaxTries=MaxTries)]
        public async Task Employee(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertEmployeeMaps(view, viewVariant));
        }
        
        [RetryTestCaseSource(nameof(CustomerVariants),MaxTries=MaxTries)]
        public async Task Customer(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertCustomerMaps(view, viewVariant));
        }

        [RetryTestCaseSource(nameof(ProductVariants),MaxTries=MaxTries)]
        public async Task Product(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertProductMaps(view, viewVariant));
        }

        [RetryTestCaseSource(nameof(OrderVariants),MaxTries=MaxTries)]
        public async Task Order(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertOrderMaps(view, viewVariant));
        }

        [RetryTestCaseSource(nameof(OpportunityVariants),MaxTries=MaxTries)]
        public async Task Opportunity(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertOpportunityMaps(view, viewVariant));
        }
    }
}