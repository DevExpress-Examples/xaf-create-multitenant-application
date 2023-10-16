using System.Collections;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using NUnit.Framework;
using OutlookInspired.Blazor.Tests.Common;
using XAF.Testing.RX;
using XAF.Testing.XAF;

#pragma warning disable CS8974 

namespace OutlookInspired.Blazor.Tests{
    public class Blazor:TestBase{
        
#if TEST
        [OutlookInspired.Tests.Common.RetryTestCaseSource(nameof(TestCases),MaxTries = 3)]
        [Category("BlazorTest")]
#else
        [TestCaseSource(nameof(BlazorTestCases))]
#endif
        public async Task Test(string navigationView, string viewVariant,string user,Func<XafApplication,string,string,IObservable<Frame>> assert){
            await StartBlazorApp(user, application => application.AssertNavigation("EmployeeListView")
                    .AssertChangeViewVariant("EmployeeCardListView")
                    .Catch<Frame,Exception>(exception => exception.Throw<Frame>()).ToUnit(),
                browser:Environment.GetEnvironmentVariable("XAFTESTBrowser"),moveToInactiveWindow:true);
        }

        public static IEnumerable BlazorTestCases 
            => TestCases.Cast<object>().Take(1);
        
        [SetUp]
        public void Setup(){

#if TEST
            XAF.Testing.RX.UtilityExtensions.TimeoutInterval=TimeSpan.FromSeconds(120);
#else
            // this.Await(async () => await LogContext.None.WriteAsync());
#endif
        }
        
    }
}