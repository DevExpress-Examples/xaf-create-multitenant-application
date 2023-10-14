using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Common;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Assert{
    static class EmployeeExtensions{
        public static IObservable<Frame> AssertEmployeeListView(this XafApplication application, string navigationView, string viewVariant) 
            => application.AssertNavigationItems((action, item) => action.AssertNavigationItems(item))
                .If(action => action.CanNavigate(navigationView), action => action.AssertEmployeeListView( navigationView, viewVariant));


        private static IObservable<Frame> AssertEmployeeListView(this SingleChoiceAction action,string navigationView, string viewVariant){
            var employeeTab = action.Application.AssertTabbedGroup(typeof(Employee),2,view => view.Model.IsDefault());
            return action.Application.AssertDashboardMasterDetail(navigationView, viewVariant,
                    existingObjectDetailview: frame => employeeTab.AssertEmployeeDetailView(frame).ToUnit(), assert: frame => frame.AssertAction())
                .Merge(employeeTab.To<Frame>().IgnoreElements()).ReplayFirstTake()
                .AssertEmployeeDashboardChildView(action.Application, viewVariant)
                .AssertMapItAction(typeof(Employee), frame => frame.AssertNestedListView(typeof(RoutePoint), assert: _ => AssertAction.HasObject))
                .AssertDashboardViewShowInDocumentAction(choiceAction => choiceAction.AssertDashboardViewShowInDocumentActionItems())
                .AssertFilterAction(filtersCount: 7,frame => frame.ClearFilter())
                .FilterListViews(action.Application);
        }

        internal static IObservable<Frame> AssertEmployeeDetailView(this IObservable<ITabControlProvider> source, Frame nestedFrame)
            => nestedFrame.AssertNestedEmployeeTask( ).IgnoreElements()
                .Concat(source.AssertNestedListView(nestedFrame, typeof(Evaluation),1, _ => Observable.Empty<Unit>(), assert: frame => frame.AssertAction(nestedFrame)))
                .ReplayFirstTake();
        
        internal static IObservable<Frame> AssertEmployeeDashboardChildView(this IObservable<Frame> source,XafApplication application,string viewVariant){
            var employeeTabControl = source.WhenEmployeeTabControl( application, viewVariant);
            return source.DashboardViewItem(item => !item.MasterViewItem())
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

        internal static IObservable<Frame> AssertNestedEmployeeTask(this Frame nestedFrame){
            var tabControl = nestedFrame.Application.AssertTabbedGroup(typeof(EmployeeTask),3);
            return nestedFrame.AssertNestedListView(typeof(EmployeeTask),
                existingObjectDetailview => tabControl.AssertRootEmployeeTask(existingObjectDetailview).ToUnit(), assert: frame => frame.AssertAction(nestedFrame))
                .Merge(tabControl.To<Frame>().IgnoreElements())
                .ReplayFirstTake();
        }

        static IObservable<Frame> AssertRootEmployeeTask(this  IObservable<ITabControlProvider> tabControl,Frame nestedFrame) 
            => tabControl.AssertNestedListView(nestedFrame, typeof(TaskAttachedFile), 1, _ => Observable.Empty<Unit>(), frame => frame.AssertAction(),inlineEdit:true)
                .IgnoreElements()
                .Concat(tabControl.AssertNestedListView(nestedFrame, typeof(Employee), 2, _ => Observable.Empty<Unit>(), frame => frame.AssertAction(nestedFrame)))
                .ReplayFirstTake();
        
        static IObservable<Frame> AssertNestedEvaluation(this Frame nestedFrame)
            => nestedFrame.AssertNestedListView(typeof(Evaluation), _ => Observable.Empty<Unit>(), assert: frame => frame.AssertAction(nestedFrame));


    }
}