using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.XtraLayout;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class ProductExtensions{
        public static IObservable<Frame> AssertProductListView(this XafApplication application, string navigationView, string viewVariant,
            int reportsCount, int filtersCount){
            var productTabControl = application.AssertTabControl<TabbedGroup>(typeof(Product));
            return application.AssertDashboardMasterDetail(navigationView, viewVariant,
                    existingObjectDetailview: frame => frame.AssertProductDetailView(productTabControl))
                .FilterListViews(application)
                .Merge(productTabControl.IgnoreElements().To<Frame>())
                .AssertDashboardViewReportsAction(Module.Controllers.Products.ReportController.ReportActionId, reportsCount)
                .AssertMapItAction(typeof(Product),
                    frame => frame.AssertNestedListView(typeof(MapItem), assert: AssertAction.HasObject))
                .AssertFilterAction(filtersCount);
        }

        internal static IObservable<Unit> AssertProductDetailView(this Frame frame, IObservable<TabbedGroup> productTabControl) 
            => frame.AssertNestedOrderItems( productTabControl)
                .ReplayFirstTake();
        
        
    }
}