using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Blazor;
using NUnit.Framework;
using OutlookInspired.Blazor.Server;
using OutlookInspired.Blazor.Tests.Common;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using XAF.Testing.Blazor.XAF;
using XAF.Testing.XAF;
using TestBase = OutlookInspired.Blazor.Tests.Common.TestBase;

namespace OutlookInspired.Blazor.Tests{
    [Order(0)]
    public class NavigationTests:TestBase{
        [RetryTestCaseSource(nameof(Users),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Items_Count(string user){
            await StartTest(user, application => application.AssertNavigationItemsCount());
            // await StartTest(user, blazorApplication => blazorApplication.AssertNavigationItemsCount());
        }
        
        [RetryTestCaseSource(nameof(Users),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Active_Items(string user){
            await StartTest(user, application => application.AssertNavigationViews());
        }

        
    }
    
    
    

}