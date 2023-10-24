using System.Reactive.Linq;
using DevExpress.ExpressApp;
using NUnit.Framework;
using XAF.Testing.Win.XAF;
using TestBase = OutlookInspired.Win.Tests.Common.TestBase;


#pragma warning disable CS8974 

namespace OutlookInspired.Win.Tests{
    [Apartment(ApartmentState.STA)]
    public class Windows:TestBase{
#if TEST
        [OutlookInspired.Tests.Common.RetryTestCaseSource(nameof(TestCases),MaxTries = 3)]
        [Category("WindowsTest")]
#else
        [TestCaseSource(nameof(TestCases))]
#endif
        public async Task Test(string navigationView, string viewVariant,string user,Func<XafApplication,string,string,IObservable<Frame>> assert){
            using var application = await SetupWinApplication(useServer:false,runInMainMonitor:false);
            
            await application.StartWinTest(assert(application,navigationView, viewVariant),user,LogContext);
        }

        
    }
}