using System.Reactive.Linq;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Blazor.Tests.Common.TestBase;

namespace OutlookInspired.Blazor.Tests{
    public class NavigationTests:TestBase{
        [RetryTestCaseSource(nameof(Users),MaxTries=MaxTries)]
        public async Task Items_Count(string user){
            await StartBlazorTest(user, blazorApplication => blazorApplication.AssertNavigationItemsCount());
        }
        
        [RetryTestCaseSource(nameof(Users),MaxTries=MaxTries)]
        public async Task Active_Items(string user){
            await StartBlazorTest(user, blazorApplication => blazorApplication.AssertNavigationViews());
        }

    }
}