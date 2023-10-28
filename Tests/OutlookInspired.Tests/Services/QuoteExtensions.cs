using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Common;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Services{
    public static class QuoteExtensions{
        public static IObservable<Unit> AssertOpportunitiesView(this XafApplication application,string view, string viewVariant) 
            => application.AssertNavigation(view, viewVariant, source => 
                    source
                        .AssertDashboardListView(listViewFrameSelector: item => item.MasterViewItem())
                        .Select(frame => frame)
                        .ReplayFirstTake()
                        .ToUnit(),application.CanNavigate(view).ToUnit())
                .FilterListViews(application);    


        private static IObservable<Frame> AssertOpportunitiesView(this SingleChoiceAction action, string navigationView, string viewVariant){
            return action.Application.AssertDashboardListView(navigationView, viewVariant, listViewFrameSelector: item => item.MasterViewItem())
                .AssertMapItAction(typeof(Quote))
                .AssertFilterAction(filtersCount: 5)
                .CloseWindow(null)
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