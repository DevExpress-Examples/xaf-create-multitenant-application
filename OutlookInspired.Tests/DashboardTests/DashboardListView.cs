using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Testing.DevExpress.ExpressApp;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Mvvm.Native;
using DevExpress.XtraGrid;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Module.Services;
using OutlookInspired.Tests.ImportData.Extensions;

namespace OutlookInspired.Tests.ImportData.DashboardTests{
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

            var assertDashboardListView = application.AssertDashboardListView(navigationView, viewVariant);
            
            var assertFilterAction = AssertFilterAction(assertDashboardListView,filtersCount);
            
            application.StartWinTest(assertDashboardListView
                    .MergeToUnit(assertFilterAction)
                    // .DoNotComplete()
            );
        }

        private static IObservable<Unit> AssertFilterAction(IObservable<DashboardViewItem> source, int filtersCount){
            var action = source.Select(item => item.Frame)
                .AssertSingleChoiceAction<ViewFilter>(ViewFilterController.FilterViewActionId, filtersCount);
            var filters = action.SelectMany(filterAction => filterAction.Items<ViewFilter>().ToNowObservable()
                    .SelectManySequential(item => filterAction.Trigger(filterAction.View()
                            .AssertObjectsCount(Convert.ToInt32(Regex.Match(item.Caption, @"\((\d+)\)").Groups[1].Value)), () => item)
                        .Assert($"{nameof(AssertFilterAction)} {item}")))
                .Skip(filtersCount - 1)
                .Assert();
            var viewFilterView = filters.IgnoreElements().To< (Frame frame,Frame source)>().Concat(action.SelectMany(choiceAction 
                => choiceAction.Trigger(choiceAction.Application.AssertListViewHasObjects(typeof(ViewFilter))
                    .Select(frame => (frame,source:choiceAction.Controller.Frame)),choiceAction.Items.First)));
            var addNewFilter = viewFilterView.SelectMany(t => t.frame.View.WhenActivated().To(t))
                .SelectMany(t => {
                    var gridView = ((GridListEditor)t.frame.View.ToListView().Editor).GridView;
                    gridView.AddNewRow();
                    gridView.FocusedRowHandle = GridControl.NewItemRowHandle;
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, nameof(ViewFilter.Name), "test");
                    gridView.UpdateCurrentRow();
                    return t.frame.GetController<DialogController>().AcceptAction.Trigger(t.source.View.WhenDataSourceChanged().To(t.source));
                })
                .SelectMany(frame => frame.Action<SingleChoiceAction>(ViewFilterController.FilterViewActionId).Items<ViewFilter>()
                    .Where(item => ((ViewFilter)item.Data).Name=="test")
                    .Do(item => {
                        var objectSpaceLink = ((IObjectSpaceLink)item.Data);
                        objectSpaceLink.ObjectSpace.Delete(item.Data);
                        objectSpaceLink.ObjectSpace.CommitChanges();
                    })).Assert();
            
            return addNewFilter.ToUnit();
        }

        private static IObservable<object> AssertFilters(int filtersCount, IObservable<SingleChoiceAction> action) 
            => action.SelectMany(choiceAction => choiceAction.Items<ViewFilter>().ToNowObservable()
                    .SelectManySequential(item => choiceAction.Trigger(choiceAction.View()
                            .AssertObjectsCount(Convert.ToInt32(Regex.Match(item.Caption, @"\((\d+)\)").Groups[1].Value)), () => item)
                        .Assert($"{nameof(AssertFilterAction)} {item}")))
                .Skip(filtersCount - 1)
                .Assert();
    }
}