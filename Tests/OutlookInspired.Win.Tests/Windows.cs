using DevExpress.ExpressApp;
using Humanizer;
using NUnit.Framework;
using OutlookInspired.Win.Tests.Common;
using XAF.Testing;
using XAF.Testing.RX;
using XAF.Testing.Win.XAF;

#pragma warning disable CS8974 

namespace OutlookInspired.Win.Tests{
    
    [Apartment(ApartmentState.STA)][Order(1)]
    public class Windows:TestBase{

        
#if TEST
        [RetryTestCaseSource(nameof(TestCases),MaxTries = 3)]
        [Category("WindowsTest")]
#else
        [TestCaseSource(nameof(TestCases))]
#endif
        
        public async Task Test(string navigationView, string viewVariant,string user,Func<XafApplication,string,string,IObservable<Frame>> assert){
#if TEST
            UtilityExtensions.TimeoutInterval = 120.Seconds();
#else
            var logContext = LogContext.None;
            Console.SetOut(await Logger.Writer(logContext));
            if (logContext != LogContext.None){
                UtilityExtensions.TimeoutInterval = 1.Days();    
            }
#endif
            using var application = await SetupWinApplication(useServer:true,runInMainMonitor:false);
            application.StartWinTest(assert(application,navigationView, viewVariant),user);
        }

        


        [SetUp]
        public void Setup(){
            StopServer();
            this.Await(async () => await AppDomain.CurrentDomain.RunDotNet("/../../../../../OutlookInspired.MiddleTier","TEST",output => output.Contains("Now listening on")));
        }
        
        private static void StopServer() => AppDomain.CurrentDomain.KillAll();

        [TearDown]
        public void TearDown() => StopServer();
    }
}