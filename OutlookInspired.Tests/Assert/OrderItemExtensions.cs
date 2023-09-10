using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.XtraLayout;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class OrderItemExtensions{
        internal static IObservable<Unit> AssertNestedOrderItems(this Frame frame, IObservable<TabbedGroup> productTabControl) 
            => productTabControl.Select(group => group).AssertNestedListView(frame, typeof(OrderItem), 1,assert:AssertAction.AllButDelete);
        internal static IObservable<Unit> AssertNestedOrderItems(this Frame frame) 
            => frame.AssertNestedListView(typeof(OrderItem), assert:AssertAction.AllButDelete).ToUnit();
        
    }
}