using System.Reactive;
using DevExpress.ExpressApp;
using DevExpress.XtraLayout;
using XAF.Testing.RX;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class OrderExtensions{
        internal static IObservable<Unit> AssertOrderDetailView(this Frame frame, IObservable<TabbedGroup> orderTabControl) 
            => frame.AssertNestedOrderItems( )
                .ReplayFirstTake();    
    }
}