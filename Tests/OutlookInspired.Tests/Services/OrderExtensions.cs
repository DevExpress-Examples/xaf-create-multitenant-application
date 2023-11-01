using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Orders;
using OutlookInspired.Tests.Common;
using XAF.Testing;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Services{
    public static class OrderExtensions{
        public static IObservable<Unit> AssertOrderListView(this XafApplication application,string view, string viewVariant) 
            => application.AssertNavigation(view, viewVariant, source => {
                    var orderTabGroup = application.AssertTabbedGroup(typeof(Order),4);
                    return source.AssertSelectDashboardListViewObject().AssertDashboardListView(
                             frame => orderTabGroup.AssertRootOrder(frame), assert:frame => frame.AssertAction())
                        .Merge(orderTabGroup.To<Frame>().IgnoreElements()).ReplayFirstTake()
                        .Select(frame => frame)
                        .AssertDashboardListViewEditView(frame => ((DetailView)frame.View).AssertPdfViewer().To(frame))
                        
                        // .If(_ => viewVariant=="Detail",frame => frame.AssertDashboardViewGridControlDetailViewObjects(nameof(Order.OrderItems)),frame => frame.Observe())
                        // .ReplayFirstTake()
                        .ToUnit();
                },application.CanNavigate(view).ToUnit())
                .FilterListViews(application);    

        

        // private static IObservable<Frame> AsserOrderListView(this SingleChoiceAction action, string navigationView, string viewVariant){
        //     var orderTabGroup = action.Application.AssertTabbedGroup(typeof(Order),4);
        //     return action.Application.AssertDashboardListView(navigationView, viewVariant,
        //             existingObjectDetailview: frame => orderTabGroup.AssertRootOrder(frame),assert:frame => frame.AssertAction())
        //         .Merge(orderTabGroup.To<Frame>().IgnoreElements()).ReplayFirstTake()
        //         .AssertDashboardListViewEditView(frame => ((DetailView)frame.View).AssertPdfViewer().To(frame))
        //         .If(_ => viewVariant=="Detail",frame => frame.AssertDashboardViewGridControlDetailViewObjects(nameof(Order.OrderItems)),frame => frame.Observe())
        //         .ReplayFirstTake()
        //         .AssertOrderReportsAction()
        //         .AssertMapItAction(typeof(Order), frame => ((DetailView)frame.View).AssertPdfViewer().To(frame))
        //         
        //         
        //         .Select(frame => frame)
        //         .AssertFilterAction(filtersCount: 7)
        //         .FilterListViews(action.Application);
        // }

        internal static IObservable<Frame> AssertOrderReportsAction(this IObservable<Frame> source) 
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .AssertSingleChoiceAction(ReportController.ReportActionId, (action, item) => action.AssertReportActionItems(item))
                .AssertReports((_, item) => item.ParentItem is{ Data: null })
                .AssertOrderInvoice().IgnoreElements().Concat(source).ReplayFirstTake();

        internal static IObservable<Frame> AssertOrderInvoice(this IObservable<SingleChoiceAction> source)
            => source.SelectMany(action => action.Items.SelectManyRecursive(item => item.Items).Where(item => item.ParentItem==null&&!item.Items.Any()).ToNowObservable()
                .SelectManySequential(item => action.Trigger(action.Application.WhenFrame(typeof(Order),ViewType.DetailView).Take(1)
                    .SelectUntilViewClosed(frame => ((DetailView)frame.View).AssertRichEditControl().To(frame)
                        .CloseWindow(null).To(action.Frame())),() => item)));
        
        // internal static IObservable<Unit> AssertOrderDetailView(this Frame frame) 
            // => frame.AssertNestedOrderItems( ).ReplayFirstTake();

        public static IObservable<Frame> AssertNestedOrder(this IObservable<ITabControlProvider> source,Frame nestedFrame,int tabIndex){
            var orderTabGroup = nestedFrame.Application.AssertTabbedGroup(typeof(Order),4);
            return source.AssertNestedListView(nestedFrame, typeof(Order), tabIndex, existingObjectDetailView 
                        => orderTabGroup.AssertRootOrder(existingObjectDetailView), frame => frame.AssertAction(nestedFrame))
                .Merge(orderTabGroup.To<Frame>().IgnoreElements()).ReplayFirstTake().Select(frame => frame);
        }
        
        

        private static IObservable<Unit> AssertRootOrder(this IObservable<ITabControlProvider> orderTabGroup,Frame nestedFrame){
            return Observable.Empty<Unit>();
            return orderTabGroup.AssertNestedListView(nestedFrame, typeof(OrderItem), 1,
                assert: frame => frame.AssertAction(nestedFrame)).ToUnit();
        }
    }
}