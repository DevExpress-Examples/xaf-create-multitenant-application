using System.Reactive;
using DevExpress.ExpressApp;
using XAF.Testing;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Services{
    public static class MailMergeExtensions{
        public static IObservable<Unit> AssertEmployeeMailMerge(this XafApplication application,string view, string viewVariant) 
            => application.AssertNavigation(view, viewVariant, source => source.AssertSelectDashboardListViewObject()
                .AssertDashboardViewShowInDocumentAction(choiceAction => choiceAction.AssertDashboardViewShowInDocumentActionItems()).ToUnit(),
                application.CanNavigate(view).ToUnit());
    }
}