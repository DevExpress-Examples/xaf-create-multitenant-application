using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl.EF;
using Humanizer;
using OutlookInspired.Module.Features.Customers;
using XAF.Testing;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.Services{
    public static class ReportsExtensions{
        public static IObservable<Unit> AssertCustomerReports(this XafApplication application,string view, string viewVariant) 
            => application.AssertReports( view, viewVariant, ReportController.ReportActionId);
        
        public static IObservable<Unit> AssertProductReports(this XafApplication application,string view, string viewVariant) 
            => application.AssertReports( view, viewVariant, Module.Features.Products.ReportController.ReportActionId);

        public static IObservable<Unit> AssertReports(this XafApplication application) 
            => application.AssertNavigation("ReportDataV2_ListView")
                .AssertListViewHasObjects()
                .Zip(application.WhenFrame(typeof(ReportDataV2),ViewType.ListView)).ToSecond()
                .SelectMany(frame => frame.View.ToListView().WhenObjects().Take(1)
                    .SelectMany(_ => frame.View.ToListView().Objects<ReportDataV2>()
                        .SelectManySequential(frame.AssertReports)
                        .BufferUntilCompleted()))
                .Take(1).ToUnit();

        private static IObservable<Unit> AssertReports(this Frame frame, ReportDataV2 reportDataV2) 
            => frame.View.ObjectSpace.GetRequiredService<IObjectSelector<ReportDataV2>>()
                .SelectObject(frame.View.ToListView(),reportDataV2).Take(1)
                .SelectMany(dataV2 => frame.AssertSimpleAction(frame.GetController<ReportsControllerCore>().Actions.First().Id)
                    .SelectMany(action => action.Trigger(action.Frame().AssertReport(dataV2.DisplayName)))
                    // .SelectMany(action => action.Trigger(frame.Application.WhenFrame("ReportViewer_DetailView").DelayOnContext(10).CloseWindow(frame)))
                )
                .Take(1).ToUnit()
            ;

        public static IObservable<Unit> AssertOrderReports(this XafApplication application,string view, string viewVariant) 
            => application.AssertReports( view, viewVariant, Module.Features.Orders.ReportController.ReportActionId,(_, item) =>  item.ParentItem is{ Data: null });

        private static IObservable<Unit> AssertReports(this XafApplication application, string view, string viewVariant, string reportActionId,Func<SingleChoiceAction,ChoiceActionItem, bool> itemSelector = null) 
            => application.AssertNavigation(view, viewVariant, source => source.AssertSelectDashboardListViewObject()
                .AssertDashboardViewReportsAction(reportActionId,itemSelector, singleChoiceAction => singleChoiceAction.AssertReportActionItems()).ToUnit(),
                application.CanNavigate(view).ToUnit());
    }
}