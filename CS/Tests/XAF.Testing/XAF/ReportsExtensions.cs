using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.Diagnostics;
using DevExpress.XtraReports.UI;

namespace XAF.Testing.XAF{
    public static class ReportsExtensions{
        public static XtraReport LoadReport<TReport>(this TReport report) where TReport:IReportDataV2 
            => ReportDataProvider.GetReportStorage(((IObjectSpaceLink)report).ObjectSpace.ServiceProvider).LoadReport(report);
        
        public static IObservable<XtraReport> Validate(this IObservable<XtraReport> source)
            => source.If(report => new ReportErrorDetector(report).Detect().Any(data => data.ErrorType == ErrorType.Error),
                report => Observable.Throw<XtraReport>(new Exception($"{report.DisplayName} has errors")),
                report => report.Observe());
        
        public static IObservable<Unit> AssertReports(this XafApplication application,Func<SimpleAction,IObservable<Unit>> showReportDesigner,IObservable<Unit> canNavigate) 
            => application.AssertReportsNavigation(source => source.SelectMany(window => window.AssertListViewHasObject().Take(1).To(window))
                    .SelectMany(frame => frame.View.ToListView().Objects<ReportDataV2>()
                        .SelectManySequential(reportDataV2 => frame.AssertReports(reportDataV2, showReportDesigner).Take(1))
                        .BufferUntilCompleted()).ToUnit(),canNavigate)
                .ReplayFirstTake().ToUnit()
        ;

        private static IObservable<Unit> AssertReports(this Frame frame, ReportDataV2 reportDataV2,Func<SimpleAction,IObservable<Unit>> showReportDesigner) 
            => frame.View.ToListView().SelectObject(frame.View.ObjectSpace.GetObject(reportDataV2)).Take(1)
                .AssertReportExecution(frame)
                .AssertDesigner(showReportDesigner,reportDataV2)
                .ToUnit();

        private static IObservable<Unit> AssertDesigner(this IObservable<Frame> source,
            Func<SimpleAction, IObservable<Unit>> showReportDesigner, ReportDataV2 reportDataV2)
            => source.ConcatIgnored(frame => frame.View.ToListView().SelectObject(frame.View.ObjectSpace.GetObject(reportDataV2)).Take(1))
                .SelectMany(frame => frame.CopyPredefinedReport().SelectMany(reportData => frame.AssertDesigner( showReportDesigner, reportData))
                .SelectMany(_ => frame.GetController<DeleteObjectsViewController>().DeleteAction.Trigger().DelayOnContext().Select(unit => unit)));
                
        private static IObservable<Unit> AssertDesigner(this Frame frame, Func<SimpleAction, IObservable<Unit>> showReportDesigner, ReportDataV2 reportData) 
            => frame.View.ToListView().SelectObject(reportData)
                .SelectMany(_ => frame.AssertSimpleAction(EditReportControllerCore.ShowReportDesignerActionId))
                .SelectMany(action => action.Trigger(showReportDesigner(action)));

        private static IObservable<ReportDataV2> CopyPredefinedReport(this Frame frame)
            => frame.AssertSimpleAction(CopyPredefinedReportsController.CopyPredefinedReportActionId)
                .SelectMany(action => action.Trigger(frame.View.ObjectSpace.WhenReloaded()
                    .SelectMany(_ => frame.View.ToListView().Objects<ReportDataV2>().WhenDefault(v2 => v2.IsPredefined))));


        private static IObservable<Frame> AssertReportExecution(this IObservable<ReportDataV2> source, Frame frame) 
            => source.SelectMany(dataV2 => frame.AssertSimpleAction(frame.GetController<ReportsControllerCore>().Actions.First().Id)
                .SelectMany(action => action.Trigger(frame.AssertReportExecution(dataV2.DisplayName).To(frame))));
        
        public static IObservable<XtraReport> WhenReady(this XtraReport report) 
            => report.PrintingSystem
                .WhenEvent<ExceptionEventArgs>(nameof(PrintingSystemBase.CreateDocumentException))
                .Select(e => e.Exception).Buffer(report.WhenEvent(nameof(XtraReport.AfterPrint))).Take(1)
                .ObserveOnContext()
                .Select(exceptions => (exceptions: exceptions.Count, pages: report.Pages.Count))
                .WhenDefault(t => t.exceptions).WhenNotDefault(t => t.pages).To(report);
    }
}