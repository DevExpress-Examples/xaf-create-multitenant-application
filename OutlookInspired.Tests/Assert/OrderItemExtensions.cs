using System.Reactive;
using DevExpress.ExpressApp;
using DevExpress.XtraLayout;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class OrderItemExtensions{
        internal static IObservable<Unit> AssertNestedOrderItems(this Frame frame, IObservable<TabbedGroup> productTabControl) 
            => productTabControl.AssertNestedListView(frame, typeof(OrderItem), 1,assert:AssertAction.AllButDelete);
        
    }
}