﻿using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Reactive.Threading.Tasks;
using DevExpress.ExpressApp;
using Humanizer;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Assert;
using OutlookInspired.Tests.Common;
using XAF.Testing.RX;
using XAF.Testing.XAF;

#pragma warning disable CS8974 

namespace OutlookInspired.Tests{
    [Apartment(ApartmentState.STA)][Order(1)]
    public class WindowsTests:TestBase{
        
#if TEST
        [RetryTestCaseSource(nameof(TestCases),MaxTries = 3)]
#else
        [TestCaseSource(nameof(TestCases))]
#endif
        public async Task Test(string navigationView, string viewVariant,string user,Func<XafApplication,string,string,IObservable<Frame>> assert) {
            
            using var application = await SetupWinApplication(useServer:false,runInMainMonitor:true);
            
            application.StartWinTest(assert(application,navigationView, viewVariant),user);
            // application.StartWinTest(1.Seconds().Delay().ToObservable(),user);
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
                             .Select(data => data.Value)
                             // .IgnoreElements()
                             .Prepend("Admin").Take(1)
                         ){
                // yield return new TestCaseData("EmployeeListView","EmployeeListView",user, AssertEmployeeListView);
                yield return new TestCaseData("EmployeeListView","EmployeeCardListView",user, AssertEmployeeListView);
                // yield return new TestCaseData("CustomerListView","CustomerListView",user,AssertCustomerListView);
                // yield return new TestCaseData("CustomerListView","CustomerCardListView",user, AssertCustomerListView);
                // yield return new TestCaseData("ProductListView","ProductCardView",user, AssertProductListView);
                // yield return new TestCaseData("ProductListView","ProductListView",user, AssertProductListView);
                // yield return new TestCaseData("OrderListView","OrderListView",user, AssertOrderListView);
                // yield return new TestCaseData("OrderListView","Detail",user, AssertOrderListView);
                // yield return new TestCaseData("Evaluation_ListView",null,user, AssertEvaluation);
                // yield return new TestCaseData("Opportunities",null,user,AssertOpportunitiesView);
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
        
        [SetUp]
        public void Setup(){
            Console.WriteLine("Setup");
            StopServer();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => {
                if (errors == SslPolicyErrors.None)
                    return true;
                Console.WriteLine("ServerCertificateValidationCallback invoked");
                return false;
            };
            var process = new Process{
                StartInfo = new ProcessStartInfo{
                    FileName = "dotnet",
                    Arguments = "run --configuration TEST",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}/../../../../../OutlookInspired.MiddleTier")
                }
            };

            process.Start();

            // Wait for the process to start by checking its output
            var processStarted = false;
            while (!processStarted){
                var output = process.StandardOutput.ReadLine();
                if (output != null && output.Contains("Now listening on")) processStarted = true;
                Console.Write(output);
                
            }

            // Now the process has started
        }
        
        private static void StopServer()
        {
            var processes = Process.GetProcessesByName("OutlookInspired.MiddleTier");

            foreach (var process in processes)
            {
                process.Kill();
                process.WaitForExit(); // Wait for the process to exit
            }
        }

        [TearDown]
        public void TearDown() => StopServer();
    }
}