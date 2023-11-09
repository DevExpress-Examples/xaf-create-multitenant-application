using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Common;
using XAF.Testing;
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
        
        public static IObservable<Frame> AssertNestedOrder(this IObservable<ITabControlProvider> source,Frame nestedFrame,int tabIndex){
            var orderTabGroup = nestedFrame.Application.AssertTabbedGroup(typeof(Order),4);
            return source.AssertNestedListView(nestedFrame, typeof(Order), tabIndex, existingObjectDetailView 
                        => orderTabGroup.AssertRootOrder(existingObjectDetailView), frame => frame.AssertAction(nestedFrame))
                .Merge(orderTabGroup.To<Frame>().IgnoreElements()).ReplayFirstTake().Select(frame => frame);
        }
        
        

        private static IObservable<Unit> AssertRootOrder(this IObservable<ITabControlProvider> orderTabGroup,Frame nestedFrame) 
            => orderTabGroup.AssertNestedListView(nestedFrame, typeof(OrderItem), 1,
                assert: frame => frame.AssertAction(nestedFrame)).ToUnit();
    }
}