using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Win.Tests.Common.TestBase;

namespace OutlookInspired.Win.Tests{
    [Apartment(ApartmentState.STA)]
    public class FilterManagerTests:TestBase{
        [RetryTestCaseSource(nameof(EmployeeVariants),MaxTries=MaxTries)]
        //[Category(WindowsTest)]
        public async Task Employee(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertEmployeeFilters(view, viewVariant));
        }
        
        [RetryTestCaseSource(nameof(CustomerVariants),MaxTries=MaxTries)]
        //[Category(WindowsTest)]
        public async Task Customer(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertCustomerFilters(view, viewVariant));
        }

        [RetryTestCaseSource(nameof(ProductVariants),MaxTries=MaxTries)]
        //[Category(WindowsTest)]
        public async Task Product(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertProductFilters(view, viewVariant));
        }

        [RetryTestCaseSource(nameof(OrderVariants),MaxTries=MaxTries)]
        //[Category(WindowsTest)]
        public async Task Order(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertOrderFilters(view, viewVariant));
        }

        [RetryTestCaseSource(nameof(OpportunityVariants),MaxTries=MaxTries)]
        //[Category(WindowsTest)]
        public async Task Opportunity(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertOpportunityFilters(view, viewVariant));
        }
    }
}