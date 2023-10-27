using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Common;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Services{
    public static class ProductExtensions{
        public static IObservable<Unit> AssertProductListView(this XafApplication application,string view, string viewVariant) 
            => application.AssertNavigation(view, viewVariant, source => {
                    var productTabControl = application.AssertTabbedGroup(typeof(Product), 2);
                    return source.AssertDashboardMasterDetail(frame => productTabControl.AssertProductDetailView(frame).ToUnit())
                        .Merge(productTabControl.IgnoreElements().To<Frame>()).ReplayFirstTake().ToUnit();
                },application.CanNavigate(view).ToUnit())
                .FilterListViews(application);    

        // public static IObservable<Frame> AssertProductListView(this XafApplication application, string navigationView, string viewVariant) 
        //     => application.AssertNavigationItems((action, item) => action.NavigationItems(item))
        //         .If(action => action.CanNavigate(navigationView), action => action.AssertProductListView( navigationView, viewVariant));

        // private static IObservable<Frame> AssertProductListView(this SingleChoiceAction action, string navigationView, string viewVariant){
        //     var productTabControl = action.Application.AssertTabbedGroup(typeof(Product), 2);
        //     return action.Application.AssertDashboardMasterDetail(navigationView, viewVariant,
        //             existingObjectDetailview: frame => productTabControl.AssertProductDetailView(frame).ToUnit())
        //         .Merge(productTabControl.IgnoreElements().To<Frame>()).ReplayFirstTake()
        //         .AssertDashboardViewReportsAction(ReportController.ReportActionId, reportsCount: singleChoiceAction => singleChoiceAction.AssertReportActionItems())
        //         .AssertMapItAction(typeof(Product), frame => frame.AssertNestedListView(typeof(MapItem), assert: _ => AssertAction.HasObject))
        //         .AssertFilterAction(filtersCount: 9)
        //         .FilterListViews(action.Application);
        // }

        internal static IObservable<Frame> AssertProductDetailView(this IObservable<ITabControlProvider> productTabControl,Frame frame){
            
            return productTabControl.AssertNestedOrderItems(frame).ReplayFirstTake();
        }
    }
}