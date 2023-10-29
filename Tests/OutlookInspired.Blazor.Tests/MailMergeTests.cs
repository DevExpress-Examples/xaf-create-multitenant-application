using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Blazor.Tests.Common.TestBase;


namespace OutlookInspired.Blazor.Tests{
    [Order(20)]
    public class MailMergeTests : TestBase{
        [RetryTestCaseSource(nameof(EmployeeVariants),MaxTries=MaxTries)]
        //[Category(Tests)]
        public async Task Employee(string user,string view,string viewVariant){
            await StartTest(user, application => application.AssertEmployeeMailMerge(view, viewVariant));
        }
    }
}