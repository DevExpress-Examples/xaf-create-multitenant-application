using System.Collections;
using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.XtraLayout;
using DevExpress.XtraPdfViewer;
using Humanizer;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.ImportData.Assert;
using OutlookInspired.Win.Editors;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData{
    [Apartment(ApartmentState.STA)]
    public class UITests:TestBase{
        [TestCaseSource(nameof(TestCases))]
        public async Task Test(string navigationView, string viewVariant,Func<XafApplication,string,string,IObservable<Frame>> assert) {
            UtilityExtensions.AssertDelayOnContextInterval = 400.Milliseconds();
            // UtilityExtensions.AssertDelayOnContextInterval = 2.Seconds();
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
                // yield return new TestCaseData("OrderListView","OrderListView", AssertOrderListView);
                // yield return new TestCaseData("OrderListView","Detail", AssertOrderListView);
                // yield return new TestCaseData("Evaluation_ListView",null, AssertEvaluation);
                // yield return new TestCaseData("Opportunities",null,AssertOpportunitiesView);
            }
        }
        
        public static IObservable<Frame> AssertEvaluation(XafApplication application, string navigationView, string viewVariant) 
            => application.AssertListView(navigationView, viewVariant);
        
        internal static IObservable<Frame> AssertOpportunitiesView(XafApplication application,string navigationView,string viewVariant) 
            => application.AssertDashboardListView(navigationView, viewVariant,listViewFrameSelector:item => item.MasterViewItem())
                .FilterListViews(application).DelayOnContext().Select(frame => frame)
                .AssertFilterAction(4)
                .CloseWindow()
                .ConcatDefer(() => application.AssertDashboardListView(navigationView, viewVariant,listViewFrameSelector:item => !item.MasterViewItem(),assert:AssertAction.AllButProcess));

        static IObservable<Frame> AssertProductListView(XafApplication application,string navigationView,string viewVariant){
            UtilityExtensions.TimeoutInterval = 60.Seconds();
            var productTabControl = application.AssertTabControl<TabbedGroup>(typeof(Product));
            return application.AssertDashboardMasterDetail(navigationView, viewVariant,existingObjectDetailview: frame => frame.AssertProductDetailView(productTabControl) )
                .FilterListViews(application)
                .Merge(productTabControl.IgnoreElements().To<Frame>())
                .AssertFilterAction(7);
        }
        
        static IObservable<Frame> AssertOrderListView(XafApplication application,string navigationView,string viewVariant){
            UtilityExtensions.TimeoutInterval = 30.Seconds();
            return application.AssertDashboardListView(navigationView, viewVariant,existingObjectDetailview: frame => frame.AssertOrderDetailView(Observable.Empty<TabbedGroup>()))
                .AssertDashboardListViewEditView(frame => ((DetailView)frame.View).AssertPdfViewerHasPages().To(frame))
                .FilterListViews(application)
                .AssertFilterAction(12);
        }


        static IObservable<Frame> AssertEmployeeListView(XafApplication application,string navigationView,string viewVariant){
            UtilityExtensions.TimeoutInterval = 60.Seconds();
            return application.AssertDashboardMasterDetail(navigationView, viewVariant,
                    existingObjectDetailview: frame => frame.AssertEmployeeDetailView())
                .AssertEmployeeDashboardChildView(application,viewVariant)
                .AssertFilterAction(5)
                .FilterListViews(application); 
        }

        static IObservable<Frame> AssertCustomerListView(XafApplication application,string navigationView,string viewVariant){
            UtilityExtensions.TimeoutInterval = 30.Seconds();
            var customerTabControl = application.AssertTabControl<TabbedGroup>(typeof(Customer));
            return application
                .AssertDashboardMasterDetail(navigationView,viewVariant, existingObjectDetailview: frame => customerTabControl.AssertCustomerDetailView(frame))
                .AssertFilterAction(5)
                .FilterListViews(application)
                .Merge(customerTabControl.IgnoreElements().To<Frame>());
            
        }

        
        
    }
}