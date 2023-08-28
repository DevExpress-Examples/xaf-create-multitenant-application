using System.Reactive.Linq;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class FilterAction{
        internal static IObservable<Frame> AssertFilterAction(this IObservable<Frame> source, int filtersCount)
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .AssertSingleChoiceAction<ViewFilter>(ViewFilterController.FilterViewActionId, filtersCount)
                .AssertFilterAction(filtersCount);

        private static IObservable<Frame> AssertFilterAction(this IObservable<SingleChoiceAction> source, int filtersCount) 
            => source.AssertFilters(filtersCount).IgnoreElements()
                .Concat(source.AssertItemsAdded(source.AssertDialogControllerListView(typeof(ViewFilter), AssertAction.All^AssertAction.Process, true).ToSecond()));
        
        private static IObservable<Frame> AssertFilters(this IObservable<SingleChoiceAction> source,int filtersCount) 
            => source.SelectMany(filterAction => filterAction.Items<ViewFilter>().ToNowObservable()
                    .SelectManySequential(item => filterAction.Trigger(filterAction.View()
                            .AssertObjectsCount(Convert.ToInt32(Regex.Match(item.Caption, @"\((\d+)\)").Groups[1].Value)), () => item)
                        .Assert($"{nameof(AssertFilters)} {item}")).To(filterAction.Frame()))
                .Skip(filtersCount - 1)
                .Assert();
    }
}