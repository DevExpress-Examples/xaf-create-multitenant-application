using System.Reactive;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Services;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Assert{
    static class OrderItemExtensions{
        internal static IObservable<Frame> AssertNestedOrderItems(this Frame nestedFrame, IObservable<ITabControlProvider> productTabControl) 
            => productTabControl.AssertNestedListView(nestedFrame, typeof(OrderItem), 1,assert:frame => frame.AssertAction(nestedFrame));
        internal static IObservable<Unit> AssertNestedOrderItems(this Frame nestedFrame) 
            => nestedFrame.AssertNestedListView(typeof(OrderItem), assert:frame => frame.AssertAction(nestedFrame)).ToUnit();
        
    }
}