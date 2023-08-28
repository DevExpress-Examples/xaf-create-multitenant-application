using System.Reactive;
using DevExpress.ExpressApp;
using DevExpress.XtraLayout;
using XAF.Testing.RX;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class ProductExtensions{
        internal static IObservable<Unit> AssertProductDetailView(this Frame frame, IObservable<TabbedGroup> productTabControl) 
            => frame.AssertNestedOrderItems( productTabControl)
                .ReplayFirstTake();
        
        
    }
}