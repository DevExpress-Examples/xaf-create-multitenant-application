using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Common;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Services{
    public static class CRUDExtensions{
        public static IObservable<Unit> AssertEmployeeCRUD(this XafApplication application,string view, string viewVariant) 
            => application.AssertNavigation(view, viewVariant, source => {
                var employeeTab = application.AssertTabbedGroup(typeof(Employee),2,detailView => detailView.Model.IsDefault());
                return source.AssertDashboardMasterDetail(frame => employeeTab.AssertEmployeeDetailView(frame).ToUnit(), assert: frame => frame.AssertAction())
                    .Merge(application.ConfigureNewEmployee().To<Window>().Merge(employeeTab.To<Window>().IgnoreElements()))
                    .ReplayFirstTake()
                    .FilterListViews(application)
                    .ToUnit();
                // return source.AssertDashboardMasterDetail(
                //             existingObjectDetailview: frame => employeeTab.AssertEmployeeDetailView(frame).ToUnit(), assert: frame => frame.AssertAction())
                //         .Merge(application.ConfigureNewEmployee().To<Window>().Merge(employeeTab.To<Window>().IgnoreElements()))
                //         .ReplayFirstTake()
                //         // .AssertEmployeeDashboardChildView(application, viewVariant)
                //         // .FilterListViews(application)
                //         .ToUnit()
                //     ;

            });    
        private static IObservable<Frame> ConfigureNewEmployee(this XafApplication application) 
            => application.WhenFrame(typeof(Employee), ViewType.DetailView).Where(frame => frame.View.IsNewObject())
                .SelectMany(frame => {
                    var applicationUser = frame.View.ObjectSpace.CreateObject<ApplicationUser>();
                    applicationUser.UserName = Guid.NewGuid().ToString();
                    return frame.View.ObjectSpace.WhenCommiting<Employee>().ToObjects()
                        .Do(employee => employee.User = applicationUser);
                }).Take(1).To<Frame>()
                .IgnoreElements();

    }
}