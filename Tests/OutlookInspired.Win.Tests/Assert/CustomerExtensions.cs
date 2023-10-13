// using System.Reactive;
// using System.Reactive.Linq;
// using DevExpress.ExpressApp;
// using DevExpress.ExpressApp.Actions;
// using DevExpress.XtraLayout;
// using OutlookInspired.Module.BusinessObjects;
// using OutlookInspired.Module.Features.Customers;
// using OutlookInspired.Tests.Common;
// using XAF.Testing.RX;
// using XAF.Testing.Win.XAF;
// using XAF.Testing.XAF;
//
// namespace OutlookInspired.Tests.Assert{
//     static class CustomerExtensions{
//         public static IObservable<Frame> AssertCustomerListView(this XafApplication application, string navigationView, string viewVariant) 
//             => application.AssertNavigationItems((action, item) => action.AssertNavigationItems(item))
//                 .If(action => action.CanNavigate(navigationView), action => action.AssertCustomerListView(navigationView, viewVariant));
//
//         private static IObservable<Frame> AssertCustomerListView(this SingleChoiceAction action, string navigationView, string viewVariant){
//             // return action.Application.AssertNavigation(navigationView).AssertChangeViewVariant(viewVariant)
//             // .AssertSelectDashboardListViewObject()
//             // // // //     .AssertGridControlDetailViewObjects(view => !new[]{ nameof(Customer.RecentOrders), nameof(Customer.Employees) }.Contains(view.LevelName)
//             // // // //         ? throw new NotImplementedException(view.LevelName) : 1).ToFirst().To<Frame>();
//             // // // .AssertDashboardViewReportsAction(ReportController.ReportActionId, reportsCount: singleChoiceAction => singleChoiceAction.AssertReportActionItems())
//             // .AssertMapItAction(typeof(Customer), frame => frame.AssertNestedListView(typeof(MapItem), assert: _ => AssertAction.HasObject))
//             // .Select(frame => frame)
//             // .ReplayFirstTake()
//             // .FilterListViews(action.Application);
//             var customerTabControl = action.Application.AssertTabbedGroup(typeof(Customer), 5);
//             return action.Application.AssertDashboardMasterDetail(navigationView, viewVariant,
//                     existingObjectDetailview: frame => customerTabControl.AssertCustomerDetailView(frame).ToUnit(),assert:frame => frame.AssertAction())
//                 .AssertFilterAction(filtersCount:7)
//                 .AssertDashboardViewReportsAction(ReportController.ReportActionId, reportsCount: singleChoiceAction => singleChoiceAction.AssertReportActionItems())
//                 .If(_ => viewVariant=="CustomerListView",frame => frame.Observe().AssertDashboardViewGridControlDetailViewObjects(nameof(Customer.RecentOrders), nameof(Customer.Employees)),frame => frame.Observe())
//                 .AssertMapItAction(typeof(Customer), frame => frame.AssertNestedListView(typeof(MapItem), assert: _ => AssertAction.HasObject))
//                 .FilterListViews(action.Application)
//                 .Merge(customerTabControl.IgnoreElements().To<Frame>());
//         }
//
//         internal static IObservable<Frame> AssertCustomerDetailView(this IObservable<TabbedGroup> source,Frame frame) 
//             => frame.Defer(() => source.AssertNestedCustomerEmployee(frame,1).IgnoreElements()
//                     .ConcatDefer(() => source.AssertNestedQuote(frame,2)).IgnoreElements()
//                     .ConcatDefer(() => source.AssertNestedCustomerStore(frame)).IgnoreElements()
//                     .ConcatDefer(() => source.AssertNestedOrder(frame,4))
//                 )
//                 .ReplayFirstTake();
//         
//         internal static IObservable<Frame> AssertNestedCustomerEmployee(this IObservable<TabbedGroup> source, Frame nestedFrame,int tabIndex)
//             => source.AssertNestedListView(nestedFrame, typeof(CustomerEmployee),tabIndex, existingObjectDetailViewFrame => 
//                 existingObjectDetailViewFrame.AssertRootCustomerEmployee(), frame =>frame.AssertAction(nestedFrame) );
//         
//         internal static IObservable<Frame> AssertNestedCustomerEmployee(this Frame nestedFrame) 
//             => nestedFrame.AssertNestedListView(typeof(CustomerEmployee), existingObjectDetailViewFrame => 
//                     existingObjectDetailViewFrame.AssertRootCustomerEmployee(),
//                 frame =>frame.AssertAction(nestedFrame) );
//         
//         private static IObservable<Frame> AssertNestedCustomerStore(this IObservable<TabbedGroup> source,Frame nestedFrame){
//             var customerStoreTabbedGroup = nestedFrame.Application.AssertTabbedGroup(typeof(CustomerStore),3);
//             return source.AssertNestedListView(nestedFrame, typeof(CustomerStore), selectedTabPageIndex: 3,
//                 frame => customerStoreTabbedGroup.AssertRootCustomerStore(frame).ToUnit(),frame => frame.AssertAction(nestedFrame))
//                 .Merge(customerStoreTabbedGroup.To<Frame>().IgnoreElements());
//         }
//
//         private static IObservable<Frame> AssertRootCustomerStore(this IObservable<TabbedGroup> source, Frame frame) 
//             => frame.Defer(() => frame.AssertNestedCustomerEmployee().IgnoreElements()
//                     .Concat(source.AssertNestedOrder(frame, 1).IgnoreElements())
//                     .Concat(source.AssertNestedQuote(frame, 2)))
//                 .Merge(source.IgnoreElements().To<Frame>());
//
//         
//         
//         static IObservable<Unit> AssertRootCustomerEmployee(this  Frame frame)
//             => frame.AssertNestedListView(typeof(CustomerCommunication),assert:frame1 => frame1.AssertAction()).ToUnit();
//         
//         
//     }
// }