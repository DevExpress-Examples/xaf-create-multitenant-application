using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Blazor.Tests.Common.TestBase;

namespace OutlookInspired.Blazor.Tests{
    [Order(0)]
    public class NavigationTests:TestBase{
        [RetryTestCaseSource(nameof(Users),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Items_Count(string user){
            await StartTest(user, application => application.AssertNavigationItemsCount());
        }
        
        [RetryTestCaseSource(nameof(Users),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Active_Items(string user){
            await StartTest(user, application => application.AssertNavigationViews());
        }

        
    }
    
    
    

}