using DevExpress.ExpressApp;

using NUnit.Framework;
using XAF.Testing;
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
            using var application = await SetupWinApplication(useServer:true,runInMainMonitor:false);
            
            application.StartWinTest(assert(application,navigationView, viewVariant),user);
        }

        [SetUp]
        public void Setup(){
            StopServer();
            this.Await(async () => await AppDomain.CurrentDomain.RunDotNet("/../../../../../OutlookInspired.MiddleTier","TEST",output => output.Contains("Now listening on")));
#if TEST
            XAF.Testing.RX.UtilityExtensions.TimeoutInterval=TimeSpan.FromSeconds(120);
#else
            this.Await(async () => await LogContext.None.WriteAsync());
#endif
        }
        
        private static void StopServer() 
            => AppDomain.CurrentDomain.KillAll("OutlookInspired.MiddleTier");

        [TearDown]
        public void TearDown() 
            => StopServer();
    }
}