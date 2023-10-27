using System.Reactive.Linq;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Blazor.Tests.Common.TestBase;


namespace OutlookInspired.Blazor.Tests{
    
    public class MailMergeTests : TestBase{
        [RetryTestCaseSource(nameof(EmployeeVariants),MaxTries=MaxTries)]
        public async Task Employee(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertEmployeeMailMerge(view, viewVariant));
        }
    }
}