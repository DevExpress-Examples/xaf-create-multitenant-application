using System.Collections;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Module.Services;
using OutlookInspired.Tests.ImportData.Extensions;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using Assert = XAF.Testing.XAF.Assert;
using TabbedGroup = DevExpress.XtraLayout.TabbedGroup;

namespace OutlookInspired.Tests.ImportData{
    [Apartment(ApartmentState.STA)]
    public class DashboardListView:TestBase{
        [TestCaseSource(nameof(TestCases))]
        public async Task Test(string navigationView, string viewVariant,int filtersCount,Func<XafApplication,string,string,int,IObservable<Frame>> assert) {
            using var application = await SetupWinApplication();
            application.Model.Options.UseServerMode = false;
            
            // var dashboardListView = assert(application);
            
            // var assertFilterAction = dashboardListView.AssertFilterAction(filtersCount);
            
            application.StartWinTest(assert(application,navigationView, viewVariant,filtersCount)
                    // .MergeToUnit(assertFilterAction)
                    // .DoNotComplete()
            );
        }
        
        private static IEnumerable TestCases{
            get{
                // yield return new TestCaseData("Role",null,-1, AssertRole);
                // yield return new TestCaseData("Users",null,-1, AssertUsers);
                // yield return new TestCaseData("Evaluation",null,-1, AssertEvaluation);
                // yield return new TestCaseData("ModelDifference",null,-1, AssertModelDifference);
                
                yield return new TestCaseData("EmployeeListView","EmployeeListView",5, AssertEmployeeListView);
                yield return new TestCaseData("EmployeeListView","EmployeeCardListView",5, AssertEmployeeListView);
                // yield return new TestCaseData("CustomerListView","CustomerListView",5, AssertCustomerListView);
                // yield return new TestCaseData("CustomerListView","CustomerCardListView",5, AssertCustomerListView);
                // yield return new TestCaseData("ProductListView","ProductCardView",7, AssertProductListView);
                // yield return new TestCaseData("ProductListView","ProductListView",7, AssertProductListView);
                // yield return new TestCaseData("OrderListView","OrderListView",12, AssertOrderListView);
                // yield return new TestCaseData("OrderListView","Detail",12, AssertOrderListView);
                // yield return new TestCaseData("Opportunities",null,4, AssertOpportunitiesView);
            }
        }

        

        public static IObservable<Frame> AssertEmployeeListView(XafApplication application,string navigationView,string viewVariant,int filterCount){
            var tabControl = application.AssertTabControl<TabbedGroup>();
            var assert = application.AssertDashboardMasterDetail(navigationView, viewVariant, assertExistingObjectDetailview: AssertEmployeeDetailView)
                .Merge(tabControl.IgnoreElements().To<Frame>().IgnoreElements())
                .ConcatIgnored(frame => frame.Observe().DashboardViewItem(item => !item.MasterViewItem()).ToFrame()
                    .SelectMany(nestedFrame => nestedFrame.AssertNestedEvaluation()
                        .ConcatDefer(() => {
                            var assertTabControl = application.AssertTabControl<TabbedGroup>();
                            return tabControl.AssertNestedListView(nestedFrame, typeof(EmployeeTask), 1, frame1 => frame1.AssertRootEmployeeTask(assertTabControl),Assert.All^Assert.Delete)
                                .Merge(assertTabControl.IgnoreElements().To<Unit>());
                        })))
                
                .DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .AssertFilterAction(filterCount);
            return application.FilterEmployeeListViews()
                .TakeUntilCompleted(assert).ToFirst().To<Frame>();
        }

        private static IObservable<Unit> AssertEmployeeDetailView(Frame employeeDetailViewFrame) 
            => employeeDetailViewFrame.Defer(() => {
                    // return Observable.Empty<Unit>();
                    var tabControl = employeeDetailViewFrame.Application.AssertTabControl<TabbedGroup>();
                    return employeeDetailViewFrame.AssertNestedEmployeeTask( tabControl)
                        .Concat(employeeDetailViewFrame.AssertNestedEvaluation())
                        .Merge(tabControl.To<Unit>().IgnoreElements());
                })
                .TakeAndReplay(1).RefCount();

        


        // public static IObservable<Frame> AssertEvaluation(XafApplication application, string navigationView, string viewVariant, int filterCount){
        //     return Observable.Empty<Frame>();
        // }
        // public static IObservable<Frame> AssertUsers(XafApplication application, string navigationView, string viewVariant, int filterCount){
        //     return Observable.Empty<Frame>();
        // }
        // public static IObservable<Frame> AssertRole(XafApplication application, string navigationView, string viewVariant, int filterCount){
        //     return Observable.Empty<Frame>();
        // }
        // public static IObservable<Frame> AssertModelDifference(XafApplication application, string navigationView, string viewVariant, int filterCount){
        //     return Observable.Empty<Frame>();
        // }
        // internal static IObservable<Frame> AssertCustomerListView(XafApplication application,string navigationView,string viewVariant,int filterCount){
        //     return application.AssertDashboardMasterDetail(navigationView, viewVariant)
        //         .MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount);
        //     // return itemSource.AssertDetailViewGridControlHasObjects().ToUnit();
        // }
        // internal static IObservable<Frame> AssertProductListView(XafApplication application,string navigationView,string viewVariant,int filterCount) 
        //     => application.AssertDashboardMasterDetail(navigationView, viewVariant)
        //         .MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount);

        // internal static IObservable<Frame> AssertOpportunitiesView(XafApplication application,string navigationView,string viewVariant,int filterCount) 
        //     => application.AssertDashboardListView(navigationView, viewVariant,_ => Observable.Empty<Frame>())
        //         .SelectMany(frame => frame.Observe().MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount).To(frame))
        //         // .CloseWindow()
        //         // .Concat(application.AssertDashboardListView(navigationView, viewVariant,_ => Observable.Empty<Frame>(), item => item.Model.ActionsToolbarVisibility==ActionsToolbarVisibility.Hide))
        //     ;

        // internal static IObservable<Frame> AssertOrderListView(XafApplication application,string navigationView,string viewVariant,int filterCount) 
        //     => application.AssertDashboardMasterDetail(navigationView, viewVariant,
        //             frame => viewVariant=="OrderListView"?frame.DashboardListViewEditFrame():frame.DashboardDetailViewFrame())
        //         .MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount);
    }
    
    

    static class AssertExtensions{
        internal static IObservable<Unit> AssertNestedEmployeeTask(this Frame frame, IObservable<TabbedGroup> tabControl) 
            => frame.AssertNestedListView(typeof(EmployeeTask), taskDetailViewFrame => taskDetailViewFrame.AssertRootEmployeeTask(tabControl), 
                    assert: Assert.All ^ Assert.Delete).ToUnit();

        internal static IObservable<Unit> AssertRootEmployeeTask(this  Frame taskDetailViewFrame,IObservable<TabbedGroup> tabControl) 
            => tabControl.AssertNestedListView(taskDetailViewFrame, typeof(TaskAttachedFile),1,_ => Observable.Empty<Unit>(), assert:Assert.All^Assert.Process)
                .Concat(tabControl.AssertNestedListView(taskDetailViewFrame,typeof(Employee),2,_ => Observable.Empty<Unit>(),Assert.HasObject));

        internal static IObservable<Unit> AssertNestedEvaluation(this Frame frame) 
            => frame.AssertNestedListView(typeof(Evaluation), _ => Observable.Empty<Unit>(), assert: Assert.All ^ Assert.Delete).ToUnit();

        internal static IObservable<Frame> AssertFilterAction(this IObservable<Frame> source, int filtersCount)
            => source.AssertSingleChoiceAction<ViewFilter>(ViewFilterController.FilterViewActionId, filtersCount)
                .AssertFilterAction(filtersCount);

        private static IObservable<Frame> AssertFilterAction(this IObservable<SingleChoiceAction> source, int filtersCount) 
            => source.AssertFilters(filtersCount).IgnoreElements()
                .Concat(source.AssertItemsAdded(source.AssertDialogControllerListView(typeof(ViewFilter), Assert.All ^ Assert.Process ^ Assert.Delete, true).ToSecond()));

        internal static IObservable<(XafApplication application, ListViewCreatingEventArgs e)> FilterEmployeeListViews(this XafApplication application) 
            => application.WhenListViewCreating(typeof(Employee))
                .Do(t => t.e.CollectionSource.SetCriteria<Employee>(employee =>employee.Evaluations.Any()&& employee.AssignedTasks.Any(task => task.AttachedFiles.Any())))
                .Merge(application.WhenListViewCreating(typeof(EmployeeTask))
                    .Do(t => t.e.CollectionSource.SetCriteria<EmployeeTask>(employeeTask =>employeeTask.AttachedFiles.Any())));

        private static IObservable<Frame> AssertFilters(this IObservable<SingleChoiceAction> source,int filtersCount) 
            => source.SelectMany(filterAction => filterAction.Items<ViewFilter>().ToNowObservable()
                    .SelectManySequential(item => filterAction.Trigger(filterAction.View()
                            .AssertObjectsCount(Convert.ToInt32(Regex.Match(item.Caption, @"\((\d+)\)").Groups[1].Value)), () => item)
                        .Assert($"{nameof(AssertFilters)} {item}")).To(filterAction.Frame()))
                .Skip(filtersCount - 1)
                .Assert();
    }
}