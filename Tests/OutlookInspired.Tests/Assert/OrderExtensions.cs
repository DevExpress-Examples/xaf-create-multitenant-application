using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraLayout;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Orders;
using OutlookInspired.Tests.Common;
using XAF.Testing;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Assert{
    static class OrderExtensions{
        public static IObservable<Frame> AssertOrderListView(this XafApplication application, string navigationView, string viewVariant) 
            => application.AssertNavigationItems((action, item) => action.AssertNavigationItems(item))
                .If(action => action.CanNavigate(navigationView), action => action.AsserOrderListView( navigationView, viewVariant));

        private static IObservable<Frame> AsserOrderListView(this SingleChoiceAction action, string navigationView, string viewVariant){
            // return action.Application.AssertNavigation(navigationView).AssertChangeViewVariant(viewVariant)
                    // .AssertSelectDashboardListViewObject()
                    // .AssertOrderReportsAction()
                    // .FilterListViews(action.Application)
                // .AssertDashboardListViewEditView(frame => ((DetailView)frame.View).AssertPdfViewer().To(frame))
                // .FilterListViews(action.Application)
                // .AssertDashboardViewGridControlDetailViewObjects(nameof(Order.OrderItems));
                
            //     // .AssertFilterAction(filtersCount);
            //     .AssertMasterFrame().ToFrame()
            //     .AssertGridControlDetailViewObjects().To<Frame>();
            // .AssertListView(assert: AssertAction.HasObject);
            // .AssertMapItAction(typeof(Order), frame => ((DetailView)frame.View).AssertPdfViewer().To(frame));

            return action.Application.AssertDashboardListView(navigationView, viewVariant,
                    existingObjectDetailview: frame => frame.AssertOrderDetailView(),assert:frame => frame.AssertAction())
                // .AssertDashboardListViewEditView(frame => ((DetailView)frame.View).AssertPdfViewer().To(frame))
                // .AssertOrderReportsAction()
                // .AssertMapItAction(typeof(Order), frame => ((DetailView)frame.View).AssertPdfViewer().To(frame))
                // .If(frame => viewVariant=="Detail",frame => frame.Observe().AssertDashboardViewGridControlDetailViewObjects(nameof(Order.OrderItems)),frame => frame.Observe())
                .AssertFilterAction(filtersCount: 12)
                .FilterListViews(action.Application);
        }

        internal static IObservable<Frame> AssertOrderReportsAction(this IObservable<Frame> source) 
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .AssertSingleChoiceAction(ReportController.ReportActionId, (action, item) => action.AssertReportActionItems(item))
                .AssertReports((_, item) => item.ParentItem is{ Data: null })
                .AssertOrderInvoice().IgnoreElements().Concat(source).ReplayFirstTake();

        internal static IObservable<Frame> AssertOrderInvoice(this IObservable<SingleChoiceAction> source)
            => source.SelectMany(action => action.Items.SelectManyRecursive(item => item.Items).Where(item => item.ParentItem==null&&!item.Items.Any()).ToNowObservable()
                .SelectManySequential(item => action.Trigger(action.Application.WhenFrame(typeof(Order),ViewType.DetailView).Take(1)
                    .SelectUntilViewClosed(frame => ((DetailView)frame.View).AssertRichEditControl().To(frame)
                        .CloseWindow().To(action.Frame())),() => item)));
        
        internal static IObservable<Unit> AssertOrderDetailView(this Frame frame) 
            => frame.AssertNestedOrderItems( ).ReplayFirstTake();

        public static IObservable<Unit> AssertNestedOrder(this IObservable<TabbedGroup> source,Frame nestedFrame,int tabIndex) 
            => source.AssertNestedListView(nestedFrame, typeof(Order),tabIndex,existingObjectDetailView => 
                existingObjectDetailView.AssertNestedListView(typeof(OrderItem),assert:frame => frame.AssertAction(nestedFrame)).ToUnit(),frame =>frame.AssertAction(nestedFrame) );
    }
}