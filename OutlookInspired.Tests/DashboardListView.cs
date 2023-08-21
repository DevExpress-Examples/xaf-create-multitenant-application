using System.Collections;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
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
            application.WhenCommitted<ViewFilter>(ObjectModification.All).Select(t => t).ToObjects().IgnoreElements()
                .To<Frame>()
                .Subscribe();
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
                // yield return new TestCaseData("EmployeeListView","EmployeeCardListView",5, AssertEmployeeListView);
                // yield return new TestCaseData("CustomerListView","CustomerListView",5, AssertCustomerListView);
                // yield return new TestCaseData("CustomerListView","CustomerCardListView",5, AssertCustomerListView);
                // yield return new TestCaseData("ProductListView","ProductCardView",7, AssertProductListView);
                // yield return new TestCaseData("ProductListView","ProductListView",7, AssertProductListView);
                // yield return new TestCaseData("OrderListView","OrderListView",12, AssertOrderListView);
                // yield return new TestCaseData("OrderListView","Detail",12, AssertOrderListView);
                // yield return new TestCaseData("Opportunities",null,4, AssertOpportunitiesView);
            }
        }

        public static IObservable<Frame> AssertEvaluation(XafApplication application, string navigationView, string viewVariant, int filterCount){
            return Observable.Empty<Frame>();
        }
        public static IObservable<Frame> AssertUsers(XafApplication application, string navigationView, string viewVariant, int filterCount){
            return Observable.Empty<Frame>();
        }
        public static IObservable<Frame> AssertRole(XafApplication application, string navigationView, string viewVariant, int filterCount){
            return Observable.Empty<Frame>();
        }
        public static IObservable<Frame> AssertModelDifference(XafApplication application, string navigationView, string viewVariant, int filterCount){
            return Observable.Empty<Frame>();
        }

        public static IObservable<Frame> AssertEmployeeListView(XafApplication application,string navigationView,string viewVariant,int filterCount){
            return application.AssertDashboardMasterDetail(navigationView, viewVariant, assertExistingObjectDetailview:AssertEmployeeDetailView)
                    .MasterDashboardViewItem().ToFrame()
                    // .AssertFilterAction(filterCount)
                ;
            
        }

        private static IObservable<Unit> AssertEmployeeDetailView(Frame frame){
            return frame.Defer(() => {
                var assertTabControl = frame.Application.AssertTabControl<TabbedGroup>();
                // return frame.AssertNestedListView(typeof(Evaluation), assert: Assert.All ^ Assert.Delete).ToUnit()
                return Observable.Empty<Unit>()
                    .Concat(frame.AssertNestedListView(typeof(EmployeeTask), 
                            AssertEmployeeTaskDetailview(assertTabControl), assert: Assert.All ^ Assert.Delete).ToUnit()
                        .Merge(assertTabControl.To<Unit>()));
            });
        }

        private static Func<Frame, IObservable<Unit>> AssertEmployeeTaskDetailview(IObservable<TabbedGroup> assertTabControl){
            return employeeTaskDetailViewFrame => employeeTaskDetailViewFrame.AssertNestedListView(typeof(TaskAttachedFile),assert:Assert.All^Assert.Process)
                .MergeToUnit(assertTabControl.Do(group => group.SelectedTabPageIndex=1));
        }

        internal static IObservable<Frame> AssertCustomerListView(XafApplication application,string navigationView,string viewVariant,int filterCount){
            return application.AssertDashboardMasterDetail(navigationView, viewVariant)
                .MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount);
            // return itemSource.AssertDetailViewGridControlHasObjects().ToUnit();
        }
        internal static IObservable<Frame> AssertProductListView(XafApplication application,string navigationView,string viewVariant,int filterCount) 
            => application.AssertDashboardMasterDetail(navigationView, viewVariant)
                .MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount);

        internal static IObservable<Frame> AssertOpportunitiesView(XafApplication application,string navigationView,string viewVariant,int filterCount) 
            => application.AssertDashboardListView(navigationView, viewVariant,_ => Observable.Empty<Frame>())
                .SelectMany(frame => frame.Observe().MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount).To(frame))
                // .CloseWindow()
                // .Concat(application.AssertDashboardListView(navigationView, viewVariant,_ => Observable.Empty<Frame>(), item => item.Model.ActionsToolbarVisibility==ActionsToolbarVisibility.Hide))
            ;

        internal static IObservable<Frame> AssertOrderListView(XafApplication application,string navigationView,string viewVariant,int filterCount) 
            => application.AssertDashboardMasterDetail(navigationView, viewVariant,
                    frame => viewVariant=="OrderListView"?frame.DashboardListViewEditFrame():frame.DashboardDetailViewFrame())
                .MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount);
    }
    
    

    static class DashboardListViewExtensions{
        internal static IObservable<Frame> AssertFilterAction(this IObservable<Frame> source, int filtersCount)
            => source.AssertSingleChoiceAction<ViewFilter>(ViewFilterController.FilterViewActionId, filtersCount)
                .AssertFilterAction(filtersCount);

        private static IObservable<Frame> AssertFilterAction(this IObservable<SingleChoiceAction> source, int filtersCount) 
            => source.AssertFilters(filtersCount).IgnoreElements()
                .Concat(source.AssertItemsAdded(source.AssertDialogControllerListView(typeof(ViewFilter), Assert.All ^ Assert.Process ^ Assert.Delete, true).ToSecond()));


        private static IObservable<Frame> AssertFilters(this IObservable<SingleChoiceAction> source,int filtersCount) 
            => source.SelectMany(filterAction => filterAction.Items<ViewFilter>().ToNowObservable()
                    .SelectManySequential(item => filterAction.Trigger(filterAction.View()
                            .AssertObjectsCount(Convert.ToInt32(Regex.Match(item.Caption, @"\((\d+)\)").Groups[1].Value)), () => item)
                        .Assert($"{nameof(AssertFilters)} {item}")).To(filterAction.Frame()))
                .Skip(filtersCount - 1)
                .Assert();
    }
}