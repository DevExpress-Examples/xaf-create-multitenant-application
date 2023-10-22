using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Products;
using OutlookInspired.Tests.Common;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Assert{
    static class ProductExtensions{
        public static IObservable<Frame> AssertProductListView(this XafApplication application, string navigationView, string viewVariant) 
            => application.AssertNavigationItems((action, item) => action.NavigationItems(item))
                .If(action => action.CanNavigate(navigationView), action => action.AssertProductListView( navigationView, viewVariant));

        private static IObservable<Frame> AssertProductListView(this SingleChoiceAction action, string navigationView, string viewVariant){
            var productTabControl = action.Application.AssertTabbedGroup(typeof(Product), 2);
            return action.Application.AssertDashboardMasterDetail(navigationView, viewVariant,
                    existingObjectDetailview: frame => frame.AssertProductDetailView(productTabControl).ToUnit())
                .Merge(productTabControl.IgnoreElements().To<Frame>()).ReplayFirstTake()
                .AssertDashboardViewReportsAction(ReportController.ReportActionId, reportsCount: singleChoiceAction => singleChoiceAction.AssertReportActionItems())
                .AssertMapItAction(typeof(Product), frame => frame.AssertNestedListView(typeof(MapItem), assert: _ => AssertAction.HasObject))
                .AssertFilterAction(action.Application,filtersCount:9)
                .FilterListViews(action.Application);
        }

        internal static IObservable<Frame> AssertProductDetailView(this Frame frame, IObservable<ITabControlProvider> productTabControl) 
            => frame.AssertNestedOrderItems( productTabControl).ReplayFirstTake();
        
        
    }
}