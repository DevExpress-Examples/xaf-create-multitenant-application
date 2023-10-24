using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Assert;
using OutlookInspired.Tests.Common;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using static OutlookInspired.Module.ModelUpdaters.DashboardViewsModelUpdater;
using static OutlookInspired.Module.ModelUpdaters.NavigationItemsModelUpdater;

namespace OutlookInspired.Tests.Services{
    public static class EmployeeExtensions{
        public static IObservable<Frame> AssertEmployeeListView(this XafApplication application, string navigationView, string viewVariant) 
            => application.AssertNavigationItems((action, item) => action.NavigationItems(item))
                .If(action => action.CanNavigate(navigationView), action => action.AssertEmployeeListView( navigationView, viewVariant));


        private static IObservable<Frame> AssertEmployeeListView(this SingleChoiceAction action,string navigationView, string viewVariant){
            var employeeTab = action.Application.AssertTabbedGroup(typeof(Employee),2,view => view.Model.IsDefault());
            // return action.Application.AssertNavigation("test")
                // .Catch<Frame,Exception>(exception => Observable.Throw<Frame>(exception));
                //     .AssertSelectDashboardListViewObject()
                //     .AssertEmployeeDashboardChildView(action.Application, viewVariant).Select(frame => frame)
                //     .AssertMapItAction(typeof(Employee), frame => frame.AssertNestedListView(typeof(RoutePoint), assert: _ => AssertAction.HasObject))
                //     .AssertDashboardViewShowInDocumentAction(choiceAction => choiceAction.AssertDashboardViewShowInDocumentActionItems())
                // .AssertFilterAction(action.Application, filtersCount: 7, frame => frame.ClearFilter());
            //     .FilterListViews(action.Application)
            //     ;
            
            // .AssertFilterAction(action.Application,filtersCount: 7,frame => frame.ClearFilter())
            // .Select(frame => frame);
            // return action.Application.FilterListViews().SelectMany(application =>
            //     application.AssertDashboardMasterDetail(navigationView, viewVariant,
            //             existingObjectDetailview: frame => employeeTab.AssertEmployeeDetailView(frame).ToUnit(), assert: frame => frame.AssertAction())
            //         .Merge(action.Application.ConfigureNewEmployee().Merge(employeeTab.To<Frame>().IgnoreElements()))
            //         .ReplayFirstTake().Select(frame => frame)
            //         .AssertEmployeeDashboardChildView(action.Application, viewVariant).Select(frame => frame)
            //         .AssertMapItAction(typeof(Employee), frame => frame.AssertNestedListView(typeof(RoutePoint), assert: _ => AssertAction.HasObject))
            //         .AssertDashboardViewShowInDocumentAction(choiceAction => choiceAction.AssertDashboardViewShowInDocumentActionItems())
            //         .AssertFilterAction(action.Application,filtersCount: 8,frame => frame.ClearFilter()));
            return action.Application.AssertDashboardMasterDetail(navigationView, viewVariant,
                    existingObjectDetailview: frame => employeeTab.AssertEmployeeDetailView(frame).ToUnit(), assert: frame => frame.AssertAction())
                .Merge(action.Application.ConfigureNewEmployee().Merge(employeeTab.To<Frame>().IgnoreElements()))
                .ReplayFirstTake()
                .AssertEmployeeDashboardChildView(action.Application, viewVariant)
                .AssertMapItAction(typeof(Employee), frame => frame.AssertNestedListView(typeof(RoutePoint), assert: _ => AssertAction.HasObject))
                .AssertDashboardViewShowInDocumentAction(choiceAction => choiceAction.AssertDashboardViewShowInDocumentActionItems())
                .AssertFilterAction(action.Application,filtersCount: 7,frame => frame.ClearFilter())
                .FilterListViews(action.Application)
                ;
        }

        private static IObservable<Frame> ConfigureNewEmployee(this XafApplication application) 
            => application.WhenFrame(typeof(Employee), ViewType.DetailView).Where(frame => frame.View.IsNewObject())
                .SelectMany(frame => {
                    var applicationUser = frame.View.ObjectSpace.CreateObject<ApplicationUser>();
                    applicationUser.UserName = Guid.NewGuid().ToString();
                    return frame.View.ObjectSpace.WhenCommiting<Employee>().ToObjects()
                        .Do(employee => employee.User = applicationUser);
                }).Take(1).To<Frame>()
                .IgnoreElements();

        internal static IObservable<Frame> AssertEmployeeDetailView(this IObservable<ITabControlProvider> source, Frame detailViewFrame){
            return detailViewFrame.AssertNestedEmployeeTask().IgnoreElements()
                .ConcatDefer(() => source.AssertNestedListView(detailViewFrame, typeof(Evaluation), 1, _ => Observable.Empty<Unit>(),
                    assert: frame => frame.AssertAction(detailViewFrame)))
                .ReplayFirstTake()
                .Select(frame => frame);
        }

        internal static IObservable<Frame> AssertEmployeeDashboardChildView(this IObservable<Frame> source,XafApplication application,string viewVariant){
            var employeeTabControl = source.WhenEmployeeTabControl( application, viewVariant);
            return source.AssertSelectDashboardListViewObject()
                .DashboardViewItem(item => !item.MasterViewItem())
                .Merge(employeeTabControl.IgnoreElements().To<DashboardViewItem>())
                .SelectMany(item => item.Frame.Observe()
                    .SelectMany(nestedFrame => nestedFrame.AssertNestedEvaluation().IgnoreElements()
                    .ConcatDefer(() => {
                        var employeeTaskTabControl = application.AssertTabbedGroup(typeof(EmployeeTask),3);
                        return employeeTabControl.AssertNestedListView(nestedFrame, typeof(EmployeeTask),1,
                                frame => employeeTaskTabControl.AssertRootEmployeeTask(frame).ToUnit(),frame => frame.AssertAction())
                            .Merge(employeeTaskTabControl.IgnoreElements().To<Frame>()).IgnoreElements();
                    }).To<Frame>()))
                .Concat(source).ReplayFirstTake();
        }

        private static IObservable<ITabControlProvider> WhenEmployeeTabControl(this IObservable<Frame> source, XafApplication application, string viewVariant) 
            => application.WhenDashboardViewTabbedGroup( viewVariant,typeof(Employee),2)
                .Replay(1).AutoConnect()
                .TakeUntil(source.DashboardViewItem(item => !item.MasterViewItem())
                    .ToFrame().ToDetailView().SelectMany(view => view.NestedListViews(typeof(EmployeeTask))).Take(1));

        internal static IObservable<Frame> AssertNestedEmployeeTask(this Frame detailViewFrame){
            // return Observable.Empty<Frame>();
            var tabControl = detailViewFrame.Application.AssertTabbedGroup(typeof(EmployeeTask),3);
            return detailViewFrame.AssertNestedListView(typeof(EmployeeTask),
                existingObjectDetailview => tabControl.AssertRootEmployeeTask(existingObjectDetailview).ToUnit(), assert: frame => frame.AssertAction(detailViewFrame))
                .Merge(tabControl.To<Frame>().IgnoreElements())
                .ReplayFirstTake();
        }

        static IObservable<Frame> AssertRootEmployeeTask(this  IObservable<ITabControlProvider> tabControl,Frame nestedFrame){
            // return Observable.Empty<Frame>();
            return tabControl.AssertNestedListView(nestedFrame, typeof(TaskAttachedFile), 1,
                    _ => Observable.Empty<Unit>(), frame => frame.AssertAction(), inlineEdit: true)
                .IgnoreElements()
                .Concat(tabControl.AssertNestedListView(nestedFrame, typeof(Employee), 2, _ => Observable.Empty<Unit>(),
                    frame => frame.AssertAction(nestedFrame)))
                .ReplayFirstTake();
        }

        static IObservable<Frame> AssertNestedEvaluation(this Frame nestedFrame){
            // return Observable.Empty<Frame>();
            return nestedFrame.AssertNestedListView(typeof(Evaluation), _ => Observable.Empty<Unit>(), assert: frame
                => frame.AssertAction(nestedFrame));
        }
        
        public static string[] NavigationViews(this ApplicationUser user) 
            => (user.Employee?.Department switch{
                EmployeeDepartment.Sales => new[]{ CustomerListView, EmployeeListView, Opportunities, OrderListView, ProductListView,WelcomeDetailView, ApplicationUserDetailView },
                EmployeeDepartment.HumanResources => new[]{ WelcomeDetailView, EmployeeListView, EvaluationListView, ApplicationUserDetailView },
                EmployeeDepartment.Support => new[]{ WelcomeDetailView, CustomerListView, Opportunities, ApplicationUserDetailView },
                EmployeeDepartment.Shipping or EmployeeDepartment.IT => new[]{ WelcomeDetailView, CustomerListView, OrderListView, ApplicationUserDetailView },
                EmployeeDepartment.Engineering => new[]{ WelcomeDetailView, EmployeeListView, CustomerListView, ApplicationUserDetailView },
                EmployeeDepartment.Management => new[]{ WelcomeDetailView, EmployeeListView,EvaluationListView, CustomerListView, ApplicationUserDetailView },
                _ => new[]{ CustomerListView, EmployeeListView, Opportunities, OrderListView, ProductListView,
                    WelcomeDetailView, ApplicationUserDetailView, EvaluationListView,RoleListView,UserListView,ModelDifferenceListView,ReportDataV2ListView,RichTextMailMergeDataListView}
            }).OrderBy(view => view).ToArray();
    }
}