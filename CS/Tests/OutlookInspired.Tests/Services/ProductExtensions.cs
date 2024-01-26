using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Common;
using XAF.Testing;
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

        
        internal static IObservable<Frame> AssertProductDetailView(this IObservable<ITabControlProvider> productTabControl,Frame frame) 
            => productTabControl.AssertNestedOrderItems(frame).ReplayFirstTake();
    }
}