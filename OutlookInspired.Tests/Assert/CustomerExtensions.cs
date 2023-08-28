using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.XtraLayout;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class CustomerExtensions{

        internal static IObservable<Unit> AssertCustomerDetailView(this IObservable<TabbedGroup> source,Frame frame) 
            => frame.Defer(() => frame.AssertNestedCustomerEmployee()
                    .Concat(source.AssertNestedOrder(frame)).IgnoreElements()
                    .Concat(source.AssertNestedQuote(frame)).IgnoreElements()
                    .Concat(source.AssertNestedCustomerStore(frame)).IgnoreElements()
                )
                .ReplayFirstTake();

        
        private static IObservable<Unit> AssertNestedCustomerStore(this IObservable<TabbedGroup> source,Frame frame) 
            => source.AssertNestedListView(frame, typeof(CustomerStore),3,AssertNestedCustomerEmployee,AssertAction.AllButDelete);

        private static IObservable<Unit> AssertNestedQuote(this IObservable<TabbedGroup> source,Frame frame) 
            => source.AssertNestedListView(frame, typeof(Quote),2,AssertNestedQuoteItem,AssertAction.AllButDelete);

        private static IObservable<Unit> AssertNestedQuoteItem(this Frame frame) 
            => frame.AssertNestedListView(typeof(QuoteItem),assert:AssertAction.AllButDelete).ToUnit();

        private static IObservable<Unit> AssertNestedOrder(this IObservable<TabbedGroup> source,Frame frame) 
            => source.AssertNestedListView(frame, typeof(Order),1,existingObjectDetailView => 
                existingObjectDetailView.AssertNestedListView(typeof(OrderItem),assert:AssertAction.AllButDelete).ToUnit(),AssertAction.AllButDelete);

        internal static IObservable<Unit> AssertNestedCustomerEmployee(this Frame frame) 
            => frame.AssertNestedListView(typeof(CustomerEmployee), existingObjectDetailViewFrame => 
                existingObjectDetailViewFrame.AssertRootCustomerEmployee(),AssertAction.AllButDelete^AssertAction.Save^AssertAction.New).ToUnit();
        static IObservable<Unit> AssertRootCustomerEmployee(this  Frame frame)
            => frame.AssertNestedListView(typeof(CustomerCommunication),assert:AssertAction.AllButDelete).ToUnit();
        
        [Obsolete]
        internal static IObservable<Frame> AssertCustomerDashboardChildView(this IObservable<Frame> source,XafApplication application){
            return source;
        }
    
        
    }
}