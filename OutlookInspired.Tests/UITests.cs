using System.Collections;
using DevExpress.ExpressApp;
using NUnit.Framework;
using OutlookInspired.Tests.ImportData.Assert;
using XAF.Testing.XAF;
#pragma warning disable CS8974 

namespace OutlookInspired.Tests.ImportData{
    [Apartment(ApartmentState.STA)]
    public class UITests:TestBase{
        [TestCaseSource(nameof(TestCases))]
        public async Task Test(string navigationView, string viewVariant,Func<XafApplication,string,string,IObservable<Frame>> assert) {
            using var application = await SetupWinApplication();
            application.Model.Options.UseServerMode = false;
            application.StartWinTest(assert(application,navigationView, viewVariant));
        }
        
        private static IEnumerable TestCases{
            get{
                yield return new TestCaseData("EmployeeListView","EmployeeListView", AssertEmployeeListView);
                yield return new TestCaseData("EmployeeListView","EmployeeCardListView", AssertEmployeeListView);
                yield return new TestCaseData("CustomerListView","CustomerListView",AssertCustomerListView);
                yield return new TestCaseData("CustomerListView","CustomerCardListView", AssertCustomerListView);
                yield return new TestCaseData("ProductListView","ProductCardView", AssertProductListView);
                yield return new TestCaseData("ProductListView","ProductListView", AssertProductListView);
                yield return new TestCaseData("OrderListView","OrderListView", AssertOrderListView);
                yield return new TestCaseData("OrderListView","Detail", AssertOrderListView);
                yield return new TestCaseData("Evaluation_ListView",null, AssertEvaluation);
                yield return new TestCaseData("Opportunities",null,AssertOpportunitiesView);
                yield return new TestCaseData("ReportDataV2_ListView",null,AssertReports);
            }
        }
        
        public static IObservable<Frame> AssertEvaluation(XafApplication application, string navigationView, string viewVariant) 
            => application.AssertListView(navigationView, viewVariant);

        internal static IObservable<Frame> AssertReports(XafApplication application, string navigationView, string viewVariant)
            => application.AssertReports(navigationView, viewVariant, reportsCount: 11);
        internal static IObservable<Frame> AssertOpportunitiesView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertOpportunitiesView( navigationView, viewVariant,filtersCount: 6);

        static IObservable<Frame> AssertProductListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertProductListView( navigationView, viewVariant, reportsCount: 4, filtersCount: 9);

        static IObservable<Frame> AssertOrderListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertOrderListView( navigationView, viewVariant, filtersCount: 13);

        static IObservable<Frame> AssertEmployeeListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertEmployeeListView(navigationView, viewVariant, filterCount: 7);

        static IObservable<Frame> AssertCustomerListView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertCustomerListView(navigationView, viewVariant, reportsCount: 3, filtersCount: 7);
    }
}