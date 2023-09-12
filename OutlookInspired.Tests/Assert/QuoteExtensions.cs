using System.Reactive.Linq;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class QuoteExtensions{
        public static IObservable<Frame> AssertOpportunitiesView(this XafApplication application, string navigationView, string viewVariant, int filtersCount) 
            => application.AssertDashboardListView(navigationView, viewVariant,
                    listViewFrameSelector: item => item.MasterViewItem())
                .FilterListViews(application).DelayOnContext().Select(frame => frame)
                .AssertMapItAction(typeof(Quote))
                .AssertFilterAction(filtersCount)
                .CloseWindow()
                .ConcatDefer(() => application.AssertDashboardListView(navigationView, viewVariant,
                    listViewFrameSelector: item => !item.MasterViewItem(), assert: AssertAction.AllButProcess));

        internal static IObservable<Frame> AssertNestedQuoteItems(this Frame frame) 
            => frame.AssertNestedListView(typeof(QuoteItem),assert:AssertAction.AllButDelete);
    }
}