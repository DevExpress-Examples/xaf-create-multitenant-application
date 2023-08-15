using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.Editors;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Module.Services;
using OutlookInspired.Tests.ImportData.Extensions;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData{
    [Apartment(ApartmentState.STA)]
    public class DashboardListView:TestBase{
        
        [TestCase("Opportunities", null,4)]
        [TestCase("OrderListView", "Detail",12)]
        [TestCase("OrderListView", "OrderListView",12)]
        [TestCase("ProductListView", "ProductCardView",7)]
        [TestCase("ProductListView", "ProductListView",7)]
        [TestCase("CustomerListView", "CustomerCardListView",5)]
        [TestCase("CustomerListView", "CustomerListView",5)]
        [TestCase("EmployeeListView", "EmployeeCardListView",5)]
        [TestCase("EmployeeListView", "EmployeeListView",5)]
        public async Task Test(string navigationView, string viewVariant,int filtersCount) {
            using var application = await SetupWinApplication();
            application.Model.Options.UseServerMode = false;

            var dashboardListView = application.AssertDashboardListView(navigationView, viewVariant);
            var assertFilterAction = dashboardListView.AssertFilterAction(filtersCount);
            
            application.StartWinTest(dashboardListView
                    .MergeToUnit(assertFilterAction)
                    // .DoNotComplete()
            );
        }
    }

    static class DashboardListViewExtensions{
        internal static IObservable<Unit> AssertFilterAction(this IObservable<Frame> source, int filtersCount)
            => source.AssertSingleChoiceAction<ViewFilter>(ViewFilterController.FilterViewActionId, filtersCount)
                .AssertFilterAction(filtersCount);

        private static IObservable<Unit> AssertFilterAction(this IObservable<SingleChoiceAction> source, int filtersCount) 
            => source.AssertFilters(filtersCount).AssertAddNewFilter( source).ToUnit();

        private static IObservable<ChoiceActionItem> AssertAddNewFilter(this IObservable<object> filters, IObservable<SingleChoiceAction> action) 
            => filters.ViewFilterView( action)
                .SelectMany(t => t.frame.View.WhenActivated().To(t))
                .SelectMany(t => {
                    var gridView = ((GridListEditor)t.frame.View.ToListView().Editor).GridView;
                    gridView.AddNewRow((nameof(ViewFilter.Name), "test"));
                    return t.frame.GetController<DialogController>().AcceptAction.Trigger(t.source.View.WhenDataSourceChanged().To(t.source));
                })
                .SelectMany(frame => frame.Action<SingleChoiceAction>(ViewFilterController.FilterViewActionId).Items<ViewFilter>()
                    .Where(item => ((ViewFilter)item.Data).Name=="test")
                    .Do(item => {
                        var objectSpaceLink = ((IObjectSpaceLink)item.Data);
                        objectSpaceLink.ObjectSpace.Delete(item.Data);
                        objectSpaceLink.ObjectSpace.CommitChanges();
                    })).Assert();

        private static IObservable<(Frame frame, Frame source)> ViewFilterView(this IObservable<object> filters, IObservable<SingleChoiceAction> action) 
            => filters.IgnoreElements().To<(Frame frame, Frame source)>()
                .Concat(action.SelectMany(choiceAction => choiceAction.Trigger(choiceAction.Application.AssertListViewHasObjects(typeof(ViewFilter)).ToFirst()
                    .Select(frame => (frame, source: choiceAction.Controller.Frame)), choiceAction.Items.First)));

        private static IObservable<object> AssertFilters(this IObservable<SingleChoiceAction> source,int filtersCount) 
            => source.SelectMany(filterAction => filterAction.Items<ViewFilter>().ToNowObservable()
                    .SelectManySequential(item => filterAction.Trigger(filterAction.View()
                            .AssertObjectsCount(Convert.ToInt32(Regex.Match(item.Caption, @"\((\d+)\)").Groups[1].Value)), () => item)
                        .Assert($"{nameof(AssertFilterAction)} {item}")))
                .Skip(filtersCount - 1)
                .Assert();
    }
}