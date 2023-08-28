using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraLayout;
using OutlookInspired.Module.BusinessObjects;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData.Assert{
    static class EmployeeExtensions{
        internal static IObservable<Unit> AssertEmployeeDetailView(this Frame frame) 
            => frame.AssertNestedEmployeeTask( ).IgnoreElements()
                .Concat(frame.AssertNestedEvaluation())
                .ReplayFirstTake();
        internal static IObservable<Frame> AssertEmployeeDashboardChildView(this IObservable<Frame> source,XafApplication application){
            var employeeTabControl = application.AssertTabControl<TabbedGroup>(typeof(Employee));
            return source.DashboardViewItem(item => !item.MasterViewItem())
                .Merge(employeeTabControl.IgnoreElements().To<DashboardViewItem>())
                .SelectMany(item => item.Frame.Observe().SelectMany(nestedFrame => nestedFrame.AssertNestedEvaluation().IgnoreElements()
                    .ConcatDefer(() => {
                        var employeeTaskTabControl = application.AssertTabControl<TabbedGroup>(typeof(EmployeeTask));
                        return employeeTabControl.AssertNestedListView(nestedFrame, typeof(EmployeeTask), 1,
                                frame1 => frame1.AssertRootEmployeeTask(employeeTaskTabControl), AssertAction.AllButDelete)
                            .Merge(employeeTaskTabControl.IgnoreElements().To<Unit>()).IgnoreElements();
                    }).To<Frame>()))
                .ConcatDefer(() => source);
        }
        
        internal static IObservable<Unit> AssertNestedEmployeeTask(this Frame frame){
            var tabControl = frame.Application.AssertTabControl<TabbedGroup>(typeof(EmployeeTask));
            return frame.AssertNestedListView(typeof(EmployeeTask),
                existingObjectDetailview => existingObjectDetailview.AssertRootEmployeeTask(tabControl),
                assert: AssertAction.AllButDelete).ToUnit()
                .Merge(tabControl.To<Unit>().IgnoreElements())
                .ReplayFirstTake();
        }

        static IObservable<Unit> AssertRootEmployeeTask(this  Frame frame,IObservable<TabbedGroup> tabControl) 
            => tabControl.AssertNestedListView(frame, typeof(TaskAttachedFile),1,_ => Observable.Empty<Unit>(), assert:AssertAction.All^AssertAction.Process)
                .Concat(tabControl.AssertNestedListView(frame,typeof(Employee),2,_ => Observable.Empty<Unit>(),AssertAction.HasObject));

        static IObservable<Unit> AssertNestedEvaluation(this Frame frame) 
            => frame.AssertNestedListView(typeof(Evaluation), _ => Observable.Empty<Unit>(), assert: AssertAction.AllButDelete).ToUnit();


    }
}