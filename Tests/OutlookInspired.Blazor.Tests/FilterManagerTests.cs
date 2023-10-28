using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Blazor.Tests.Common.TestBase;

namespace OutlookInspired.Blazor.Tests{
    [Order(40)]
    public class FilterManagerTests:TestBase{
        [RetryTestCaseSource(nameof(EmployeeVariants),MaxTries=MaxTries)]
        public async Task Employee(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertEmployeeFilters(view, viewVariant));
        }
        
        [RetryTestCaseSource(nameof(CustomerVariants),MaxTries=MaxTries)]
        public async Task Customer(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertCustomerFilters(view, viewVariant));
        }

        [RetryTestCaseSource(nameof(ProductVariants),MaxTries=MaxTries)]
        public async Task Product(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertProductFilters(view, viewVariant));
        }

        [RetryTestCaseSource(nameof(OrderVariants),MaxTries=MaxTries)]
        public async Task Order(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertOrderFilters(view, viewVariant));
        }

        [RetryTestCaseSource(nameof(OpportunityVariants),MaxTries=MaxTries)]
        public async Task Opportunity(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertOpportunityFilters(view, viewVariant));
        }
    }
}