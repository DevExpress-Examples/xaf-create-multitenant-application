using System.Collections;
using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Templates;
using DevExpress.XtraLayout;
using Humanizer;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.ImportData.Assert;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData{
    [Apartment(ApartmentState.STA)]
    public class UITests:TestBase{
        [TestCaseSource(nameof(TestCases))]
        public async Task Test(string navigationView, string viewVariant,int filtersCount,Func<XafApplication,string,string,int,IObservable<Frame>> assert) {
            using var application = await SetupWinApplication();
            application.Model.Options.UseServerMode = false;
            application.StartWinTest(assert(application,navigationView, viewVariant,filtersCount));
        }
        
        private static IEnumerable TestCases{
            get{
                // yield return new TestCaseData("EmployeeListView","EmployeeListView",5, AssertEmployeeListView);
                // yield return new TestCaseData("EmployeeListView","EmployeeCardListView",5, AssertEmployeeListView);
                yield return new TestCaseData("CustomerListView","CustomerListView",5, AssertCustomerListView);
                yield return new TestCaseData("CustomerListView","CustomerCardListView",5, AssertCustomerListView);
                // yield return new TestCaseData("ProductListView","ProductCardView",7, AssertProductListView);
                // yield return new TestCaseData("ProductListView","ProductListView",7, AssertProductListView);
                // yield return new TestCaseData("OrderListView","OrderListView",12, AssertOrderListView);
                // yield return new TestCaseData("OrderListView","Detail",12, AssertOrderListView);
                // yield return new TestCaseData("Evaluation_ListView",null,-1, AssertEvaluation);
                // yield return new TestCaseData("Opportunities",null,4, AssertOpportunitiesView);
            }
        }
        
        public static IObservable<Frame> AssertEvaluation(XafApplication application, string navigationView, string viewVariant, int filterCount) 
            => application.AssertListView(navigationView, viewVariant);
        internal static IObservable<Frame> AssertOpportunitiesView(XafApplication application,string navigationView,string viewVariant,int filterCount) 
            => application.AssertDashboardMasterDetail(navigationView, viewVariant, _ => Observable.Empty<Frame>(),
                    existingObjectDetailview: frame => frame.AssertNestedQuoteItems().ToUnit())
                .FilterListViews(application)
                .CloseWindow()
                .Concat(application.AssertDashboardMasterDetail(navigationView, viewVariant,
                    _ => Observable.Empty<Frame>(), item => !item.MasterViewItem(),existingObjectDetailview: frame => frame.AssertNestedQuoteItems().ToUnit())
                    .FilterListViews(application))
                .AssertFilterAction(filterCount);

        static IObservable<Frame> AssertProductListView(XafApplication application,string navigationView,string viewVariant,int filterCount){
            UtilityExtensions.TimeoutInterval = 2.Minutes();
            var productTabControl = application.AssertTabControl<TabbedGroup>(typeof(Product));
            return application.AssertDashboardMasterDetail(navigationView, viewVariant,existingObjectDetailview: frame => frame.AssertProductDetailView(productTabControl) )
                .FilterListViews(application)
                .Merge(productTabControl.IgnoreElements().To<Frame>())
                .AssertFilterAction(filterCount);
        }
        
        static IObservable<Frame> AssertOrderListView(XafApplication application,string navigationView,string viewVariant,int filterCount){
            UtilityExtensions.TimeoutInterval = 2.Minutes();
            var orderTabControl = application.AssertTabControl<TabbedGroup>(typeof(Order));
            return application.AssertDashboardMasterDetail(navigationView, viewVariant,existingObjectDetailview: frame => frame.AssertOrderDetailView(orderTabControl))
                .FilterListViews(application)
                .Merge(orderTabControl.IgnoreElements().To<Frame>())
                .AssertFilterAction(filterCount);
        }

        static IObservable<Frame> AssertEmployeeListView(XafApplication application,string navigationView,string viewVariant,int filterCount){
            UtilityExtensions.TimeoutInterval = 60.Seconds();
            UtilityExtensions.AssertDelayOnContextInterval = 100.Milliseconds();
            return application
                    .AssertDashboardMasterDetail(navigationView, viewVariant,
                        existingObjectDetailview: frame => frame.AssertEmployeeDetailView())
                .AssertEmployeeDashboardChildView(application,viewVariant)
                .AssertFilterAction(filterCount)
                .FilterListViews(application); ;
        }

        static IObservable<Frame> AssertCustomerListView(XafApplication application,string navigationView,string viewVariant,int filterCount){
            UtilityExtensions.TimeoutInterval = 30.Seconds();
            UtilityExtensions.AssertDelayOnContextInterval = 250.Milliseconds();
            var customerTabControl = application.AssertTabControl<TabbedGroup>(typeof(Customer));
            var assert = application
                    .AssertDashboardMasterDetail(navigationView,viewVariant, existingObjectDetailview: frame => customerTabControl.AssertCustomerDetailView(frame))
                    .AssertFilterAction(filterCount);
            return assert
                .Merge(application.FilterAllListViews(assert))
                .Merge(customerTabControl.IgnoreElements().To<Frame>());
            
        }

        
        
    }
}