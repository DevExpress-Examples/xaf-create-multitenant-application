using System.Reactive.Linq;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Module.Features;
using OutlookInspired.Module.Features.ViewFilter;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class FilterActionExtensions{
        
        internal static IObservable<Frame> AssertFilterAction(this IObservable<Frame> source, int filtersCount)
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .AssertSingleChoiceAction(ViewFilterController.FilterViewActionId, filtersCount)
                .AssertFilterAction().IgnoreElements().Concat(source);

        private static IObservable<Frame> AssertFilterAction(this IObservable<SingleChoiceAction> source) 
            => source.AssertFilters().IgnoreElements()
                .Concat(source.AssertItemsAdded(source.AssertDialogControllerListView(typeof(ViewFilter), AssertAction.All^AssertAction.Process, true).ToSecond()));
        
        private static IObservable<Frame> AssertFilters(this IObservable<SingleChoiceAction> source) 
            => source.SelectMany(filterAction => filterAction.Items<ViewFilter>().ToNowObservable()
                    .SelectManySequential(item => filterAction.Trigger(filterAction.View()
                            .AssertObjectsCount(Convert.ToInt32(Regex.Match(item.Caption, @"\((\d+)\)").Groups[1].Value)), () => item)
                        .Assert($"{nameof(AssertFilters)} {item}")).To(filterAction.Frame()))
                .IgnoreElements().To<Frame>().Concat(source.Select(action => action.Frame())).ReplayFirstTake();
    }
}