using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Win.Tests.Common.TestBase;

namespace OutlookInspired.Win.Tests{
    [Apartment(ApartmentState.STA)]
    [Category(WindowsTest)]
    public class NavigationTests:TestBase{
        [RetryTestCaseSource(nameof(Users),MaxTries=MaxTries)]
        public async Task Items_Count(string user){
            await StartTest(user, application => application.AssertNavigationItemsCount());
        }

        [RetryTestCaseSource(nameof(Users),MaxTries=MaxTries)]
        public async Task Active_Items(string user){
            await StartTest(user, application => application.AssertNavigationViews());
        }
    }
}