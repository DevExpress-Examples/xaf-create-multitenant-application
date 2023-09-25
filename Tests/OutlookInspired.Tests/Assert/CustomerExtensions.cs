using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraLayout;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Customers;
using OutlookInspired.Tests.Common;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Assert{
    static class CustomerExtensions{
        public static IObservable<Frame> AssertCustomerListView(this XafApplication application, string navigationView, string viewVariant) 
            => application.AssertNavigationItems((action, item) => action.AssertNavigationItems(item))
                .If(action => action.CanNavigate(navigationView), action => action.AssertCustomerListView(navigationView, viewVariant));

        private static IObservable<Frame> AssertCustomerListView(this SingleChoiceAction action, string navigationView, string viewVariant){
            // return action.Application.AssertNavigation(navigationView).AssertChangeViewVariant(viewVariant)
            // .AssertSelectDashboardListViewObject()
            // // // //     .AssertGridControlDetailViewObjects(view => !new[]{ nameof(Customer.RecentOrders), nameof(Customer.Employees) }.Contains(view.LevelName)
            // // // //         ? throw new NotImplementedException(view.LevelName) : 1).ToFirst().To<Frame>();
            // // // .AssertDashboardViewReportsAction(ReportController.ReportActionId, reportsCount: singleChoiceAction => singleChoiceAction.AssertReportActionItems())
            // .AssertMapItAction(typeof(Customer), frame => frame.AssertNestedListView(typeof(MapItem), assert: _ => AssertAction.HasObject))
            // .Select(frame => frame)
            // .ReplayFirstTake()
            // .FilterListViews(action.Application);
            var customerTabControl = action.Application.AssertTabbedGroup(typeof(Customer), 4);
            return action.Application.AssertDashboardMasterDetail(navigationView, viewVariant,
                    existingObjectDetailview: customerTabControl.AssertCustomerDetailView,assert:frame => frame.AssertAction())
                .AssertFilterAction(filtersCount:7)
                .AssertDashboardViewReportsAction(ReportController.ReportActionId, reportsCount: singleChoiceAction => singleChoiceAction.AssertReportActionItems())
                .If(_ => viewVariant=="CustomerListView",frame => frame.Observe().AssertDashboardViewGridControlDetailViewObjects(nameof(Customer.RecentOrders), nameof(Customer.Employees)),frame => frame.Observe())
                .AssertMapItAction(typeof(Customer), frame => frame.AssertNestedListView(typeof(MapItem), assert: _ => AssertAction.HasObject))
                .FilterListViews(action.Application)
                .Merge(customerTabControl.IgnoreElements().To<Frame>());
        }

        internal static IObservable<Unit> AssertCustomerDetailView(this IObservable<TabbedGroup> source,Frame frame) 
            => frame.Defer(() => frame.AssertNestedCustomerEmployee().IgnoreElements()
                    .Concat(source.AssertNestedQuote(frame,1)).IgnoreElements()
                    .Concat(source.AssertNestedCustomerStore(frame)).IgnoreElements()
                    .Concat(source.AssertNestedOrder(frame,3))
                )
                .ReplayFirstTake();
        
        private static IObservable<Unit> AssertNestedCustomerStore(this IObservable<TabbedGroup> source,Frame nestedFrame){
            var customerStoreTabbedGroup = nestedFrame.Application.AssertTabbedGroup(typeof(CustomerStore),3);
            return source.AssertNestedListView(nestedFrame, typeof(CustomerStore), selectedTabPageIndex: 2,
                frame => customerStoreTabbedGroup.AssertRootCustomerStore(frame),frame => frame.AssertAction(nestedFrame))
                .MergeToUnit(customerStoreTabbedGroup);
        }

        private static IObservable<Unit> AssertRootCustomerStore(this IObservable<TabbedGroup> source, Frame frame) 
            => frame.Defer(() => frame.AssertNestedCustomerEmployee().IgnoreElements()
                    .Concat(source.AssertNestedOrder(frame, 1).IgnoreElements())
                    .Concat(source.AssertNestedQuote(frame, 2)))
                .Merge(source.IgnoreElements().To<Unit>());

        internal static IObservable<Unit> AssertNestedCustomerEmployee(this Frame nestedFrame) 
            => nestedFrame.AssertNestedListView(typeof(CustomerEmployee), existingObjectDetailViewFrame => 
                existingObjectDetailViewFrame.AssertRootCustomerEmployee(),
                frame =>frame.AssertAction(nestedFrame) ).ToUnit();
        
        static IObservable<Unit> AssertRootCustomerEmployee(this  Frame frame)
            => frame.AssertNestedListView(typeof(CustomerCommunication),assert:frame1 => frame1.AssertAction()).ToUnit();
        
        
    }
}