using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.XtraLayout;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers.Customers;
using OutlookInspired.Tests.ImportData.Extensions;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class CustomerExtensions{
        public static IObservable<Frame> AssertCustomerListView(this XafApplication application, string navigationView, string viewVariant, int reportsCount, int filtersCount){
            // return application.AssertNavigation(navigationView).AssertChangeViewVariant(viewVariant)
            //     .AssertSelectDashboardListViewObject()
            //     .AssertMapItAction(typeof(Customer), frame => frame.AssertNestedListView(typeof(MapItem), assert: AssertAction.HasObject))
            //     .FilterListViews(application);
            var customerTabControl = application.AssertTabControl<TabbedGroup>(typeof(Customer));
            return application.AssertDashboardMasterDetail(navigationView, viewVariant,
                    existingObjectDetailview: customerTabControl.AssertCustomerDetailView)
                .AssertDashboardViewReportsAction(ReportController.ReportActionId, reportsCount)
                .AssertMapItAction(typeof(Customer), frame => frame.AssertNestedListView(typeof(MapItem), assert: AssertAction.HasObject))
                .AssertFilterAction(filtersCount)
                .FilterListViews(application)
                .Merge(customerTabControl.IgnoreElements().To<Frame>());
        }

        internal static IObservable<Unit> AssertCustomerDetailView(this IObservable<TabbedGroup> source,Frame frame) 
            => frame.Defer(() => frame.AssertNestedCustomerEmployee()
                    .Concat(source.AssertNestedQuote(frame)).IgnoreElements()
                    .Concat(source.AssertNestedCustomerStore(frame)).IgnoreElements()
                    .Concat(source.AssertNestedOrder(frame)).IgnoreElements()
                )
                .ReplayFirstTake();

        
        private static IObservable<Unit> AssertNestedCustomerStore(this IObservable<TabbedGroup> source,Frame frame) 
            => source.AssertNestedListView(frame, typeof(CustomerStore),2,AssertNestedCustomerEmployee,AssertAction.AllButDelete);

        private static IObservable<Unit> AssertNestedQuote(this IObservable<TabbedGroup> source,Frame frame) 
            => source.AssertNestedListView(frame, typeof(Quote),1,AssertNestedQuoteItem,AssertAction.AllButDelete);

        private static IObservable<Unit> AssertNestedQuoteItem(this Frame frame) 
            => frame.AssertNestedListView(typeof(QuoteItem),assert:AssertAction.AllButDelete).ToUnit();

        private static IObservable<Unit> AssertNestedOrder(this IObservable<TabbedGroup> source,Frame frame) 
            => source.AssertNestedListView(frame, typeof(Order),3,existingObjectDetailView => 
                existingObjectDetailView.AssertNestedListView(typeof(OrderItem),assert:AssertAction.AllButDelete).ToUnit(),AssertAction.AllButDelete);

        internal static IObservable<Unit> AssertNestedCustomerEmployee(this Frame frame) 
            => frame.AssertNestedListView(typeof(CustomerEmployee), existingObjectDetailViewFrame => 
                existingObjectDetailViewFrame.AssertRootCustomerEmployee(),AssertAction.AllButDelete^AssertAction.DetailViewSave^AssertAction.DetailViewNew).ToUnit();
        static IObservable<Unit> AssertRootCustomerEmployee(this  Frame frame)
            => frame.AssertNestedListView(typeof(CustomerCommunication),assert:AssertAction.AllButDelete).ToUnit();
    }
}