using System.Reactive;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.Features.Customers;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Services{
    public static class ReportsExtensions{
        public static IObservable<Unit> AssertCustomerReports(this XafApplication application,string view, string viewVariant) 
            => application.AssertReports( view, viewVariant, ReportController.ReportActionId);
        
        public static IObservable<Unit> AssertProductReports(this XafApplication application,string view, string viewVariant) 
            => application.AssertReports( view, viewVariant, Module.Features.Products.ReportController.ReportActionId);
        public static IObservable<Unit> AssertOrderReports(this XafApplication application,string view, string viewVariant) 
            => application.AssertReports( view, viewVariant, Module.Features.Orders.ReportController.ReportActionId,(_, item) =>  item.ParentItem is{ Data: null });

        private static IObservable<Unit> AssertReports(this XafApplication application, string view, string viewVariant, string reportActionId,Func<SingleChoiceAction,ChoiceActionItem, bool> itemSelector = null) 
            => application.AssertNavigation(view, viewVariant, source => source.AssertSelectDashboardListViewObject()
                .AssertDashboardViewReportsAction(reportActionId,itemSelector, singleChoiceAction => singleChoiceAction.AssertReportActionItems()).ToUnit());
    }
}