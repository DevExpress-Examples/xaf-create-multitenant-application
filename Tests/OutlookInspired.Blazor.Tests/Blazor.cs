using System.Collections;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using Humanizer;
using NUnit.Framework;
using OutlookInspired.Blazor.Tests.Common;
using XAF.Testing;
using XAF.Testing.RX;


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
            // UtilityExtensions.DelayOnContextInterval = 2.Seconds();
            UtilityExtensions.TimeoutInterval = 15.Seconds();
            
            await StartBlazorTest(user, application => assert(application, navigationView, viewVariant).ToUnit(),
                    browser:Environment.GetEnvironmentVariable("XAFTESTBrowser"),inactiveMonitorLocation:WindowPosition.FullScreen)
                .Log(LogContext, inactiveMonitorLocation:WindowPosition.BottomRight,alwaysOnTop:true);
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