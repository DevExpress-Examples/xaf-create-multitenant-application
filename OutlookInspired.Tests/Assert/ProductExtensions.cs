using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraLayout;
using Humanizer;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Products;
using OutlookInspired.Tests.ImportData.Extensions;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class ProductExtensions{
        public static IObservable<Frame> AssertProductListView(this XafApplication application, string navigationView, string viewVariant) 
            => application.AssertNavigationItems((action, item) => action.AssertNavigationItems(item))
                .If(action => action.CanNavigate(navigationView), action => action.AssertProductListView( navigationView, viewVariant));

        private static IObservable<Frame> AssertProductListView(this SingleChoiceAction action, string navigationView, string viewVariant){
            // return action.Application.AssertNavigation(navigationView).AssertChangeViewVariant(viewVariant)
                // .DelayOnContext(15)
                // .FilterListViews(action.Application);
            // .AssertMapItAction(typeof(Product),
            // frame => frame.AssertNestedListView(typeof(MapItem), assert: AssertAction.HasObject));
            // UtilityExtensions.TimeoutInterval = 60.Seconds();
            var productTabControl = action.Application.AssertTabbedGroup(typeof(Product), 2);
            return action.Application.AssertDashboardMasterDetail(navigationView, viewVariant,
                    existingObjectDetailview: frame => frame.AssertProductDetailView(productTabControl))
                .FilterListViews(action.Application)
                .Merge(productTabControl.IgnoreElements().To<Frame>())
                .AssertDashboardViewReportsAction(ReportController.ReportActionId, reportsCount: singleChoiceAction => singleChoiceAction.AssertReportActionItems())
                .AssertMapItAction(typeof(Product), frame => frame.AssertNestedListView(typeof(MapItem), assert: _ => AssertAction.HasObject))
                .AssertFilterAction(filtersCount:9);
        }

        internal static IObservable<Unit> AssertProductDetailView(this Frame frame, IObservable<TabbedGroup> productTabControl) 
            => frame.AssertNestedOrderItems( productTabControl)
                .ReplayFirstTake();
        
        
    }
}