using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Common;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Assert{
    static class QuoteExtensions{
        public static IObservable<Frame> AssertOpportunitiesView(this XafApplication application, string navigationView, string viewVariant) 
            => application.AssertNavigationItems((action, item) => action.AssertNavigationItems(item))
                .If(action => action.CanNavigate(navigationView), action => action.AssertOpportunitiesView(navigationView, viewVariant));

        private static IObservable<Frame> AssertOpportunitiesView(this SingleChoiceAction action, string navigationView, string viewVariant){
            return action.Application.AssertDashboardListView(navigationView, viewVariant, listViewFrameSelector: item => item.MasterViewItem())
                .AssertMapItAction(typeof(Quote))
                .AssertFilterAction(filtersCount:5)
                .CloseWindow()
                .ConcatDefer(() => action.Application.AssertDashboardListView(navigationView, viewVariant,
                    listViewFrameSelector: item => !item.MasterViewItem(), assert: _ => AssertAction.HasObject))
                .FilterListViews(action.Application).DelayOnContext().Select(frame => frame);
            
        }
        
        public static IObservable<Frame> AssertNestedQuote(this IObservable<ITabControlProvider> source,Frame nestedFrame,int tabIndex) 
            => source.AssertNestedListView(nestedFrame, typeof(Quote),tabIndex,AssertRootQuote,frame =>frame.AssertAction(nestedFrame) )
                .SelectMany(frame => frame.Observe())
                ;

        public static IObservable<Unit> AssertRootQuote(this Frame nestedFrame) 
            => nestedFrame.AssertNestedListView(typeof(QuoteItem),assert:frame => frame.AssertAction(nestedFrame)).ToUnit();
    }
}