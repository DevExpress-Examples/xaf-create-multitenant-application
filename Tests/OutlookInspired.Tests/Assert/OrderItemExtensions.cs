using System.Reactive;
using DevExpress.ExpressApp;
using DevExpress.XtraLayout;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Assert{
    static class OrderItemExtensions{
        internal static IObservable<Unit> AssertNestedOrderItems(this Frame nestedFrame, IObservable<TabbedGroup> productTabControl) 
            => productTabControl.AssertNestedListView(nestedFrame, typeof(OrderItem), 1,assert:frame => frame.AssertAction(nestedFrame));
        internal static IObservable<Unit> AssertNestedOrderItems(this Frame nestedFrame) 
            => nestedFrame.AssertNestedListView(typeof(OrderItem), assert:frame => frame.AssertAction(nestedFrame)).ToUnit();
        
    }
}