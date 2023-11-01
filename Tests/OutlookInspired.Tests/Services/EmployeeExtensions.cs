using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;
using OutlookInspired.Tests.Common;
using XAF.Testing;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using static OutlookInspired.Module.ModelUpdaters.DashboardViewsModelUpdater;
using static OutlookInspired.Module.ModelUpdaters.NavigationItemsModelUpdater;

namespace OutlookInspired.Tests.Services{
    public static class EmployeeExtensions{
        // public static IObservable<Unit> AssertCustomerMaps(this XafApplication application,string view, string viewVariant) 
        //     => application.AssertNavigation(view, viewVariant,source => source.AssertSelectDashboardListViewObject()
        //             .AssertMapItAction(typeof(Customer), frame => frame.AssertNestedListView(typeof(MapItem), assert: _ => AssertAction.HasObject)).ToUnit(),
        //         application.CanNavigate(view).ToUnit());

        public static IObservable<Unit> AssertEmployeeListView(this XafApplication application,string view, string viewVariant) 
            => application.AssertNavigation(view, viewVariant, source => {
                var employeeTab = application.AssertTabbedGroup(typeof(Employee),2,detailView => detailView.Model.IsDefault());
                return source.AssertDashboardMasterDetail(frame => employeeTab.AssertEmployeeDetailView(frame).ToUnit(), assert: frame => frame.AssertAction())
                    .Merge(application.ConfigureClonedEmployee().To<Window>().Merge(employeeTab.To<Window>().IgnoreElements()))
                    .ReplayFirstTake()
                    .ToUnit();
            },application.CanNavigate(view).ToUnit())
            .FilterListViews(application)
            ;    
        
        static IObservable<Frame> AssertEmployeeDetailView(this IObservable<ITabControlProvider> source, Frame detailViewFrame){
            return detailViewFrame.AssertNestedEmployeeTask().IgnoreElements()
                .ConcatDefer(() => source.AssertNestedListView(detailViewFrame, typeof(Evaluation), 1,
                    _ => Observable.Empty<Unit>(),
                    assert: frame => frame.AssertAction(detailViewFrame)))
                .ReplayFirstTake()
                .Select(frame => frame);
        }

        static IObservable<Frame> AssertNestedEmployeeTask(this Frame detailViewFrame){
            var tabControl = detailViewFrame.Application.AssertTabbedGroup(typeof(EmployeeTask),3);
            return detailViewFrame.AssertNestedListView(typeof(EmployeeTask),
                    existingObjectDetailview => tabControl.AssertRootEmployeeTask(existingObjectDetailview).ToUnit(), assert: frame => frame.AssertAction(detailViewFrame))
                .Merge(tabControl.To<Frame>().IgnoreElements())
                .ReplayFirstTake();
        }

        private static IObservable<Frame> ConfigureClonedEmployee(this XafApplication application) 
            => application.WhenFrame(typeof(Employee), ViewType.DetailView).Where(frame => frame.View.IsNewObject())
                .SelectMany(frame => frame.GetController<ModificationsController>().SaveAction.WhenExecuting().ToFirst()
                    .Do(tuple => ((Employee)frame.View.CurrentObject).User = frame.View.ObjectSpace
                        .GetObjectsQuery<ApplicationUser>().First(user => user.Employee == null))).Take(1).To<Frame>()
                .IgnoreElements();
        
        static IObservable<Frame> AssertRootEmployeeTask(this  IObservable<ITabControlProvider> tabControl,Frame nestedFrame) 
            => tabControl.AssertNestedListView(nestedFrame, typeof(TaskAttachedFile), 1,
                    _ => Observable.Empty<Unit>(), frame => frame.AssertAction(), inlineEdit: true).IgnoreElements()
                .Concat(tabControl.AssertNestedListView(nestedFrame, typeof(Employee), 2, _ => Observable.Empty<Unit>(), frame => frame.AssertAction(nestedFrame)))
                .ReplayFirstTake();

        
        public static string[] NavigationViews(this ApplicationUser user) 
            => (user.Employee?.Department switch{
                EmployeeDepartment.Sales => new[]{ CustomerListView, EmployeeListView, Opportunities, OrderListView, ProductListView,WelcomeDetailView, ApplicationUserDetailView },
                EmployeeDepartment.HumanResources => new[]{ WelcomeDetailView, EmployeeListView, EvaluationListView, ApplicationUserDetailView },
                EmployeeDepartment.Support => new[]{ WelcomeDetailView, CustomerListView, Opportunities, ApplicationUserDetailView },
                EmployeeDepartment.Shipping or EmployeeDepartment.IT => new[]{ WelcomeDetailView, CustomerListView, OrderListView, ApplicationUserDetailView },
                EmployeeDepartment.Engineering => new[]{ WelcomeDetailView, EmployeeListView, CustomerListView, ApplicationUserDetailView },
                EmployeeDepartment.Management => new[]{ WelcomeDetailView, EmployeeListView,EvaluationListView, CustomerListView, ApplicationUserDetailView },
                _ => new[]{ CustomerListView, EmployeeListView, Opportunities, OrderListView, ProductListView,
                    WelcomeDetailView, ApplicationUserDetailView, EvaluationListView,RoleListView,UserListView,ModelDifferenceListView,RichTextMailMergeDataListView}
            }).OrderBy(view => view).ToArray();
    }
}