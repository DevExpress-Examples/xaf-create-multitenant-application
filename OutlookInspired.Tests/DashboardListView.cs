using System.Collections;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Win.Editors;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Module.Services;
using OutlookInspired.Tests.ImportData.Extensions;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using Assert = XAF.Testing.XAF.Assert;

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

        public static IObservable<Frame> AssertEmployeeListView(XafApplication application,string navigationView,string viewVariant,int filterCount){
            return application.AssertDashboardMasterDetail(navigationView, viewVariant,
                        assertExistingObjectDetailview:frame => frame.AssertNestedListView(typeof(Evaluation), assert:Assert.All ^ Assert.Delete).ToUnit())
                .MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount)
                ;
            // return frame.AssertNestedListView(typeof(Evaluation), Assert.All ^ Assert.Delete).ToUnit();
            // return frame.AssertNestedListView(typeof(Evaluation), Assert.All ^ Assert.Delete)
            //     .Concat(frame.AssertNestedListView(typeof(EmployeeTask), Assert.All ^ Assert.Delete))
            //     .ToUnit();
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
                .CloseWindow()
                .Concat(application.AssertDashboardListView(navigationView, viewVariant,_ => Observable.Empty<Frame>(), item => item.Model.ActionsToolbarVisibility==ActionsToolbarVisibility.Hide))
            ;

        internal static IObservable<Frame> AssertOrderListView(XafApplication application,string navigationView,string viewVariant,int filterCount) 
            => application.AssertDashboardMasterDetail(navigationView, viewVariant,
                    frame => viewVariant=="OrderListView"?frame.DashboardListViewEditFrame():frame.DashboardDetailViewFrame())
                .MasterDashboardViewItem().ToFrame().AssertFilterAction(filterCount);
    }
    
    

    static class DashboardListViewExtensions{
        internal static IObservable<Frame> AssertFilterAction(this IObservable<Frame> source, int filtersCount)
            => source.AssertSingleChoiceAction<ViewFilter>(ViewFilterController.FilterViewActionId, filtersCount)
                .AssertFilterAction(filtersCount)
                .Select(action => action.Frame())
            ;

        private static IObservable<SingleChoiceAction> AssertFilterAction(this IObservable<SingleChoiceAction> source, int filtersCount) 
            => source.AssertFilters(filtersCount)
                .AssertAddNewFilter( source)
                ;

        private static IObservable<SingleChoiceAction> AssertAddNewFilter(this IObservable<Frame> filters, IObservable<SingleChoiceAction> action) 
            => filters.ViewFilterView( action)
                .SelectMany(t => t.frame.View.WhenActivated().To(t))
                .SelectMany(t => {
                    var gridView = ((GridListEditor)t.frame.View.ToListView().Editor).GridView;
                    gridView.AddNewRow((nameof(ViewFilter.Name), "test"));
                    return t.frame.GetController<DialogController>().AcceptAction.Trigger(t.source.View.WhenDataSourceChanged().To(t.source));
                })
                .Select(frame => frame.Action<SingleChoiceAction>(ViewFilterController.FilterViewActionId))
                .SelectMany(choiceAction => choiceAction.Items<ViewFilter>()
                    .Where(item => ((ViewFilter)item.Data).Name=="test")
                    .Do(item => {
                        var objectSpaceLink = ((IObjectSpaceLink)item.Data);
                        objectSpaceLink.ObjectSpace.Delete(item.Data);
                        objectSpaceLink.ObjectSpace.CommitChanges();
                    }).To(choiceAction)).Assert();

        private static IObservable<(Frame frame, Frame source)> ViewFilterView(this IObservable<object> filters, IObservable<SingleChoiceAction> action) 
            => filters.IgnoreElements().To<(Frame frame, Frame source)>()
                .Concat(action.AssertDialogControllerListView());

        private static IObservable<(Frame frame, Frame source)> AssertDialogControllerListView(this IObservable<SingleChoiceAction> action) 
            => action.SelectMany(choiceAction => choiceAction.Trigger(choiceAction.Application.AssertListViewHasObjects(typeof(ViewFilter)).ToFirst()
                .Select(frame => (frame, source: choiceAction.Controller.Frame)), choiceAction.Items.First));

        private static IObservable<Frame> AssertFilters(this IObservable<SingleChoiceAction> source,int filtersCount) 
            => source.SelectMany(filterAction => filterAction.Items<ViewFilter>().ToNowObservable()
                    .SelectManySequential(item => filterAction.Trigger(filterAction.View()
                            .AssertObjectsCount(Convert.ToInt32(Regex.Match(item.Caption, @"\((\d+)\)").Groups[1].Value)), () => item)
                        .Assert($"{nameof(AssertFilterAction)} {item}")).To(filterAction.Frame()))
                .Skip(filtersCount - 1)
                .Assert();
    }
}