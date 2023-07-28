using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Testing.DevExpress.ExpressApp;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.ExpressApp.Win;
using DevExpress.XtraLayout;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.ImportData.Extensions;

namespace OutlookInspired.Tests.ImportData.EmployeeTests{
    [Apartment(ApartmentState.STA)]
    public class DashboardChildDetailView:TestBase{
        [TestCase("Card")]
        [TestCase("List")]
        public async Task Test(string view){
            var card = view == "Card";
            using var application = await SetupWinApplication();
            application.Model.Options.UseServerMode = false;

            var navigate = application.Navigate("EmployeeListView")
                .Replay(1).RefCount();
            var dashboardViewFrame = application.WhenFrame(ViewType.DashboardView).Cast<Window>().ReplayConnect(1).Take(1);
            
            var switchToLayoutListView = card?application.WhenFrame(ViewType.DashboardView).AssertChangeViewVariant("EmployeeCardListView"):Observable.Empty<Unit>();
            var employeeDetailViewItem = navigate.SelectMany(window => window.DashboardViewItems(ViewType.DetailView,typeof(Employee)))
                .TakeAndReplay(1).RefCount();
            var detailViewDoesNotDisplayData = DetailViewDoesNotDisplayData(employeeDetailViewItem);
            var detailViewDisplaysData = DetailViewDisplaysData(employeeDetailViewItem);
            var evaluationsExist = EvaluationsExist(application).ReplayConnect(1);
            var tasksExist = TasksExist(application).TakeAndReplay(1).RefCount();
            var tabControl = employeeDetailViewItem.TabControl<TabbedGroup>()
                .TakeAndReplay(1).RefCount()
                .TakeUntil(tasksExist.WhenCompleted());
            var selectListViewObject = dashboardViewFrame.SelectListViewObject().Take(1);
            
            application.StartWinTest(
                navigate.MergeToUnit(switchToLayoutListView
                        .ConcatToUnit(detailViewDoesNotDisplayData.ConcatToUnit(detailViewDisplaysData.Merge(selectListViewObject))
                            .MergeToUnit(tabControl)))
                .Merge(evaluationsExist.ConcatToUnit(tabControl.Do(o => o.SelectedTabPageIndex = 1)))
                .Merge(tasksExist)
                // .DoNotComplete()
            );
        }

        private static IObservable<Unit> TasksExist(XafApplication application) 
            => application.WhenFrame(typeof(EmployeeTask),ViewType.ListView).SelectMany(frame => frame.View.ToListView().Objects())
                .Assert().ToUnit();

        private static IObservable<Unit> EvaluationsExist(WinApplication application) 
            => application.WhenFrame(typeof(Evaluation),ViewType.ListView)
                .SelectMany(frame => frame.View.ToListView().Objects())
                .Assert().ToUnit();

        private static IObservable<Unit> DetailViewDisplaysData(IObservable<DashboardViewItem> employeeDetailViewItem) 
            => employeeDetailViewItem.Select(item => item).ToView<DetailView>()
                .SelectMany(detailView => detailView.WhenSelectionChanged().To(detailView))
                .WhenNotDefault(detailView => detailView.CurrentObject)
                .Assert().ToUnit();

        private static IObservable<Unit> DetailViewDoesNotDisplayData(IObservable<DashboardViewItem> employeeDetailViewItem) 
            => employeeDetailViewItem.ToView<DetailView>().WhenDefault(detailView => detailView.CurrentObject)
                .Assert().ToUnit();
    }
}