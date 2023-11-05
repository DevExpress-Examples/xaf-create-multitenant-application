using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.ViewFilter;
using XAF.Testing;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Services{
    public static class FilterManagerExtensions{
        public static IObservable<Unit> AssertEmployeeFilters(this XafApplication application,string view, string viewVariant) 
            => application.AssertFilters(view, viewVariant,7);
        public static IObservable<Unit> AssertCustomerFilters(this XafApplication application,string view, string viewVariant) 
            => application.AssertFilters(view, viewVariant,7);
        public static IObservable<Unit> AssertProductFilters(this XafApplication application,string view, string viewVariant) 
            => application.AssertFilters(view, viewVariant,9);
        public static IObservable<Unit> AssertOrderFilters(this XafApplication application,string view, string viewVariant) 
            => application.AssertFilters(view, viewVariant,12);
        public static IObservable<Unit> AssertOpportunityFilters(this XafApplication application,string view, string viewVariant) 
            => application.AssertFilters(view, viewVariant,5);
        static IObservable<Unit> AssertFilters(this XafApplication application,string view, string viewVariant,int filterCount) 
            => application.AssertNavigation(view, viewVariant,source => source
                .AssertFilterAction(filtersCount: filterCount,action: frame => frame.ClearFilter()).ToUnit(),
                application.CanNavigate(view).ToUnit());
        
        internal static IObservable<Frame> AssertFilterAction(this IObservable<Frame> source, int filtersCount, Action<NestedFrame> action = null)
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .Do(frame => action?.Invoke(frame))
                .AssertSingleChoiceAction(ViewFilterController.FilterViewActionId,_ => filtersCount)
                .AssertFilterAction().IgnoreElements().Concat(source)
                .ReplayFirstTake().Select(frame => frame);

        private static IObservable<Frame> AssertFilterAction(this IObservable<SingleChoiceAction> source) 
            => source.AssertFilters().IgnoreElements()
                .Concat(source.AssertNewFilter().Select(frame => frame)
                    .AssertDeleteFilter().Select(frame => frame)
                )
                .ReplayFirstTake();

        private static IObservable<Frame> AssertDeleteFilter(
            this IObservable<(ViewFilter filter, SingleChoiceAction action)> source)
            =>source.Do(t => t.action.SelectedItem=t.action.Items.First())
                .SelectMany(t => t.action.Application.WhenFrame(typeof(ViewFilter),ViewType.ListView).Take(1)
                    .Do(frame => frame.View.ToListView().CollectionSource.SetCriteria<ViewFilter>(filter => filter.ID==t.filter.ID)).IgnoreElements()
                    .Merge(t.action.Application.WhenCommitted<ViewFilter>(ObjectModification.All).ToObjects().Take(1).Select(filter => filter)
                        .CombineLatest(t.action.WhenItemsChanged(ChoiceActionItemChangesType.ItemsRemove).Select(args => args), (filter, _) => filter)
                        .Where(filter => t.action.Items.Select(item => item.Data).OfType<ViewFilter>().All(itemFilter => itemFilter.ID == filter.ID))
                        .Select(_ => t.action.Frame()))
                        .Merge(t.action.AssertDialogControllerListView(typeof(ViewFilter), _ => AssertAction.ListViewDeleteOnly).Select(frame => frame).IgnoreElements())
                    .Select(frame => frame))
                .Assert();
        
        private static IObservable<(ViewFilter viewFilter, SingleChoiceAction action)> AssertNewFilter(this IObservable<SingleChoiceAction> source) 
            => source.Do(action => action.SelectedItem=action.Items.First())
                .SelectMany(action => action.Application.WhenCommitted<ViewFilter>(ObjectModification.New).ToObjects().Take(1)
                    .CombineLatest(action.WhenItemsChanged(ChoiceActionItemChangesType.ItemsAdd), (filter, _) => filter)
                    .SelectMany(filter => action.Items.Select(item => item.Data).OfType<ViewFilter>().Where(itemFilter => itemFilter.ID == filter.ID)).Take(1)
                    .Merge(action.AssertDialogControllerListView(typeof(ViewFilter), _ => AssertAction.DetailViewSave, action.Application.GetRequiredService<IFilterViewManager>().InlineEdit)
                        .IgnoreElements().IgnoreElements().To<ViewFilter>())
                    .Select(filter => (filter,action)))
                .Assert();

        private static IObservable<Frame> AssertFilters(this IObservable<SingleChoiceAction> source) 
            => source.SelectMany(filterAction => filterAction.Items<ViewFilter>().ToNowObservable()
                    .SelectManySequential(item => filterAction.Trigger(filterAction.Frame()
                            .AssertObjectsCount(Convert.ToInt32(Regex.Match(item.Caption, @"\((\d+)\)").Groups[1].Value)), () => item)
                        .Assert($"{nameof(AssertFilters)} {item}")).To(filterAction.Frame()))
                .IgnoreElements().To<Frame>().Concat(source.Select(action => action.Frame())).ReplayFirstTake();
    }

    
}