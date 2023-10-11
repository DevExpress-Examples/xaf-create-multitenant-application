using System.Collections;
using DevExpress.ExpressApp;
using Humanizer;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Assert;
using OutlookInspired.Tests.Common;
using XAF.Testing;
using XAF.Testing.RX;
using XAF.Testing.XAF;

#pragma warning disable CS8974 

namespace OutlookInspired.Tests{
    
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
            if (logContext == LogContext.All){
                UtilityExtensions.TimeoutInterval = 1.Days();    
            }
#endif
            using var application = await SetupWinApplication(useServer:true,runInMainMonitor:false);
            application.StartWinTest(assert(application,navigationView, viewVariant),user);
        }

        private static readonly Dictionary<EmployeeDepartment, string> Roles = new(){
            { EmployeeDepartment.Sales, "clarkm"},{EmployeeDepartment.HumanResources,"gretas"},
            {EmployeeDepartment.Support,"jamesa"},{EmployeeDepartment.Shipping,"dallasl"},
            {EmployeeDepartment.Engineering,"barta"},{EmployeeDepartment.Management,"johnh"},{EmployeeDepartment.IT,"bradleyj"},
            {(EmployeeDepartment)(-1),"Admin"}
        };
        
        public static IEnumerable TestCases => Users()
            
            .SelectMany(TestCaseData);
        
        private static IEnumerable<TestCaseData> TestCaseData(string user){
            yield return new TestCaseData("EmployeeListView","EmployeeListView",user, AssertEmployeeListView);
            yield return new TestCaseData("EmployeeListView","EmployeeCardListView",user, AssertEmployeeListView);
            yield return new TestCaseData("CustomerListView","CustomerListView",user,AssertCustomerListView);
            yield return new TestCaseData("CustomerListView","CustomerCardListView",user, AssertCustomerListView);
            yield return new TestCaseData("ProductListView","ProductCardView",user, AssertProductListView);
            yield return new TestCaseData("ProductListView","ProductListView",user, AssertProductListView);
            yield return new TestCaseData("OrderListView","OrderListView",user, AssertOrderListView);
            yield return new TestCaseData("OrderListView","Detail",user, AssertOrderListView);
            yield return new TestCaseData("Evaluation_ListView",null,user, AssertEvaluation);
            yield return new TestCaseData("Opportunities",null,user,AssertOpportunitiesView);
            // yield return new TestCaseData("ReportDataV2_ListView",null,AssertReports)
        }

        private static IEnumerable<string> Users(){
            var roleStr = $"{Environment.GetEnvironmentVariable("TEST_ROLE")}".Split(' ').Last();
            return Enum.TryParse(roleStr, out EmployeeDepartment department) && Roles.TryGetValue(department, out var user) ? user.YieldItem() :
                roleStr == "Admin" ? "Admin".YieldItem() : Roles.Values;
        }


        public static IObservable<Frame> AssertNewUser(XafApplication application, string navigationView, string viewVariant){
            throw new NotImplementedException();
        }
        public static IObservable<Frame> AssertEvaluation(XafApplication application, string navigationView, string viewVariant) 
            => application.AssertNavigationItems((action, item) => action.AssertNavigationItems(item))
                .If(action => action.CanNavigate(navigationView), _ => application.AssertListView(navigationView, viewVariant));

        internal static IObservable<Frame> AssertReports(XafApplication application, string navigationView, string viewVariant)
            => application.AssertReports(navigationView, viewVariant, reportsCount: 11);
        internal static IObservable<Frame> AssertOpportunitiesView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertOpportunitiesView( navigationView, viewVariant);

        static IObservable<Frame> AssertProductListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertProductListView( navigationView, viewVariant);

        static IObservable<Frame> AssertOrderListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertOrderListView( navigationView, viewVariant);

        static IObservable<Frame> AssertEmployeeListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertEmployeeListView(navigationView, viewVariant);

        static IObservable<Frame> AssertCustomerListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertCustomerListView(navigationView, viewVariant);
        
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