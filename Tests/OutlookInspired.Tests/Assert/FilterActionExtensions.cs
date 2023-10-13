using System.Reactive.Linq;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.ViewFilter;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Assert{
    static class FilterActionExtensions{
        internal static IObservable<Frame> AssertFilterAction(this IObservable<Frame> source, int filtersCount,Action<NestedFrame> action=null)
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .Do(frame => action?.Invoke(frame))
                .AssertSingleChoiceAction(ViewFilterController.FilterViewActionId,_ => filtersCount)
                .AssertFilterAction().IgnoreElements().Concat(source).ReplayFirstTake();

        private static IObservable<Frame> AssertFilterAction(this IObservable<SingleChoiceAction> source) 
            => source.AssertFilters().IgnoreElements()
                .Concat(source.AssertItemsAdded()
                    .Merge(source.AssertDialogControllerListView(typeof(ViewFilter), _ => AssertAction.DetailViewDelete, true).ToSecond().IgnoreElements()))
                .ReplayFirstTake();
        
        private static IObservable<Frame> AssertFilters(this IObservable<SingleChoiceAction> source) 
            => source.SelectMany(filterAction => filterAction.Items<ViewFilter>().ToNowObservable()
                    .SelectManySequential(item => filterAction.Trigger(filterAction.View()
                            .AssertObjectsCount(Convert.ToInt32(Regex.Match(item.Caption, @"\((\d+)\)").Groups[1].Value)), () => item)
                        .Assert($"{nameof(AssertFilters)} {item}")).To(filterAction.Frame()))
                .IgnoreElements().To<Frame>().Concat(source.Select(action => action.Frame())).ReplayFirstTake();
    }
}