using System.Collections;
using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.XtraLayout;
using Humanizer;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.ImportData.Assert;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData{
    [Apartment(ApartmentState.STA)]
    public class DashboardListView:TestBase{
        [TestCaseSource(nameof(TestCases))]
        public async Task Test(string navigationView, string viewVariant,int filtersCount,Func<XafApplication,string,string,int,IObservable<Frame>> assert) {
            using var application = await SetupWinApplication();
            application.Model.Options.UseServerMode = false;
            
            // var dashboardListView = assert(application);
            
            // var assertFilterAction = dashboardListView.AssertFilterAction(filtersCount);
            
            application.StartWinTest(assert(application,navigationView, viewVariant,filtersCount)
                    // .MergeToUnit(assertFilterAction)
                    // .DoNotComplete()
            );
        }
        
        private static IEnumerable TestCases{
            get{
                // yield return new TestCaseData("Role",null,-1, AssertRole);
                // yield return new TestCaseData("Users",null,-1, AssertUsers);
                // yield return new TestCaseData("Evaluation",null,-1, AssertEvaluation);
                // yield return new TestCaseData("ModelDifference",null,-1, AssertModelDifference);
                
                // yield return new TestCaseData("EmployeeListView","EmployeeListView",5, AssertEmployeeListView);
                // yield return new TestCaseData("EmployeeListView","EmployeeCardListView",5, AssertEmployeeListView);
                // yield return new TestCaseData("CustomerListView","CustomerListView",5, AssertCustomerListView);
                // yield return new TestCaseData("CustomerListView","CustomerCardListView",5, AssertCustomerListView);
                // yield return new TestCaseData("ProductListView","ProductCardView",7, AssertProductListView);
                // yield return new TestCaseData("ProductListView","ProductListView",7, AssertProductListView);
                // yield return new TestCaseData("OrderListView","OrderListView",12, AssertOrderListView);
                yield return new TestCaseData("OrderListView","Detail",12, AssertOrderListView);
                // yield return new TestCaseData("Opportunities",null,4, AssertOpportunitiesView);
            }
        }
        
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
            UtilityExtensions.TimeoutInterval = 2.Minutes();
            return application
                .AssertDashboardMasterDetail(navigationView,viewVariant, existingObjectDetailview: frame => frame.AssertEmployeeDetailView())
                .AssertEmployeeDashboardChildView(application)
                .AssertFilterAction(filterCount)
                .FilterListViews(application);
        }

        static IObservable<Frame> AssertCustomerListView(XafApplication application,string navigationView,string viewVariant,int filterCount){
            UtilityExtensions.TimeoutInterval = 2.Minutes();
            var customerTabControl = application.AssertTabControl<TabbedGroup>(typeof(Customer));
            var assert = application
                    .AssertDashboardMasterDetail(navigationView,viewVariant, existingObjectDetailview: frame => customerTabControl.AssertCustomerDetailView(frame))
                    .AssertFilterAction(filterCount);
            return assert
                .Merge(application.FilterAllListViews(assert))
                .Merge(customerTabControl.IgnoreElements().To<Frame>());
            
        }

        // public static IObservable<Frame> AssertEvaluation(XafApplication application, string navigationView, string viewVariant, int filterCount){
        //     return Observable.Empty<Frame>();
        // }
        // public static IObservable<Frame> AssertUsers(XafApplication application, string navigationView, string viewVariant, int filterCount){
        //     return Observable.Empty<Frame>();
        // }
        // public static IObservable<Frame> AssertRole(XafApplication application, string navigationView, string viewVariant, int filterCount){
        //     return Observable.Empty<Frame>();
        // }
        // public static IObservable<Frame> AssertModelDifference(XafApplication application, string navigationView, string viewVariant, int filterCount){
        //     return Observable.Empty<Frame>();
        // }
        
        

        // internal static IObservable<Frame> AssertOpportunitiesView(XafApplication application,string navigationView,string viewVariant,int filterCount) 
        //     => application.AssertDashboardListView(navigationView, viewVariant,_ => Observable.Empty<Frame>())
        //         .SelectMany(frame => frame.Observe().MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount).To(frame))
        //         // .CloseWindow()
        //         // .Concat(application.AssertDashboardListView(navigationView, viewVariant,_ => Observable.Empty<Frame>(), item => item.Model.ActionsToolbarVisibility==ActionsToolbarVisibility.Hide))
        //     ;

        // internal static IObservable<Frame> AssertOrderListView(XafApplication application,string navigationView,string viewVariant,int filterCount) 
        //     => application.AssertDashboardMasterDetail(navigationView, viewVariant,
        //             frame => viewVariant=="OrderListView"?frame.DashboardListViewEditFrame():frame.DashboardDetailViewFrame())
        //         .MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount);
    }
}