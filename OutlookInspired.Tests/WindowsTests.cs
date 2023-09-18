using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using Humanizer;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;
using OutlookInspired.Tests.ImportData.Assert;
using OutlookInspired.Tests.ImportData.Extensions;
using XAF.Testing;
using XAF.Testing.RX;
using XAF.Testing.XAF;
#pragma warning disable CS8974 

namespace OutlookInspired.Tests.ImportData{
    [Apartment(ApartmentState.STA)]
    public class WindowsTests:TestBase{
        
        [TestCaseSource(nameof(TestCases))][Retry(3)]
        public async Task Test(string navigationView, string viewVariant,string user,Func<XafApplication,string,string,IObservable<Frame>> assert) {
            using var application = await SetupWinApplication(useServer:false,runInMainMonitor:false);
            // UtilityExtensions.DelayOnContextInterval = 2.Seconds();
            
            application.StartWinTest(assert(application,navigationView, viewVariant),user);
        }

        private static readonly Dictionary<EmployeeDepartment, string> Roles = new(){
            { EmployeeDepartment.Sales, "clarkm"},{EmployeeDepartment.HumanResources,"gretas"},
            {EmployeeDepartment.Support,"jamesa"},{EmployeeDepartment.Shipping,"dallasl"},
            {EmployeeDepartment.Engineering,"barta"},{EmployeeDepartment.Management,"johnh"},{EmployeeDepartment.IT,"bradleyj"},
        };
        
        private static IEnumerable TestCases{
            get{
                // yield return new TestCaseData("CustomerListView","CustomerCardListView","Admin",AssertCustomerListView);
                foreach (var user in Roles
                             // .IgnoreElements()
                             // .Where(pair => pair.Key==EmployeeDepartment.IT)
                             .Select(data => data.Value).Prepend("Admin")){
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
                //     // yield return new TestCaseData("ReportDataV2_ListView",null,AssertReports)
                }
            }
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
        
        [OneTimeSetUp]
        public void Setup(){
            StopServer();
            new Process{
                StartInfo = new ProcessStartInfo{
                    FileName = "dotnet",
                    Arguments = "run --no-build",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = "C:\\Work\\DX\\OutlookInspired\\OutlookInspired.MiddleTier\\"
                }
            }.Start();
        }

        private static void StopServer() 
            => Process.GetProcessesByName("OutlookInspired.MiddleTier")
                .Do(process1 => process1.Kill())
                .Enumerate();

        [OneTimeSetUp]
        public void TearDown() => StopServer();
    }
}