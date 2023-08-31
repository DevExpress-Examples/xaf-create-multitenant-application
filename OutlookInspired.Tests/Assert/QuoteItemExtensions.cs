using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class QuoteItemExtensions{
        internal static IObservable<Frame> AssertNestedQuoteItems(this Frame frame) 
            => frame.AssertNestedListView(typeof(QuoteItem),assert:AssertAction.AllButDelete);
    }
}