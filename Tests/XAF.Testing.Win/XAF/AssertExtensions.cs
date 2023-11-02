using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Office.Win;
using DevExpress.ExpressApp.ReportsV2.Win;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraMap;
using DevExpress.XtraPdfViewer;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit;
using XAF.Testing.XAF;

namespace XAF.Testing.Win.XAF{
    public static class AssertExtensions{

        public static IObservable<Frame> AssertReport(this Frame frame, [CallerMemberName]string caller="") 
            => frame.GetController<WinReportServiceController>()
                .WhenEvent<WinReportServiceController.CustomizePrintToolEventArgs>(
                    nameof(WinReportServiceController.CustomizePrintTool)).Take(1)
                .SelectMany(e => ((XtraReport)e.PrintTool.Report).WhenReady()
                    .DelayOnContext()
                    .Do(_ => e.PrintTool.PreviewRibbonForm.Close())
                    .DelayOnContext()).Assert($"{nameof(AssertReport)}",caller:caller)
                .To(frame);

        public static IObservable<PdfViewer> AssertPdfViewer(this DetailView detailView) 
            => detailView.WhenPropertyEditorControl().OfType<PdfViewer>()
                .WhenNotDefault(viewer => viewer.PageCount)
                .Assert();


        public static IObservable<RichEditControl> AssertRichEditControl(this IObservable<RichEditControl> source, bool assertMailMerge = false,[CallerMemberName]string caller="")
            => source.SelectMany(control => control.WhenEvent(nameof(control.DocumentLoaded)).To(control))
                .WhenNotDefault(control => control.DocumentLayout.GetPageCount())
                .Assert($"{caller} {nameof(RichEditControl.DocumentLoaded)}")
                .If(_ => assertMailMerge, control => control.WhenEvent(nameof(control.MailMergeFinished)).To(control)
                    .Assert($"{caller} {nameof(RichEditControl.MailMergeFinished)}"),control => control.Observe());

        public static IObservable<Frame> AssertShowInDocumentAction(this SingleChoiceAction action, ChoiceActionItem item) 
            => action.Frame().GetController<RichTextServiceController>()
                .WhenEvent<CustomRichTextRibbonFormEventArgs>(nameof(RichTextServiceController.CustomCreateRichTextRibbonForm)).Take(1)
                .Do(e => e.RichTextRibbonForm.WindowState = FormWindowState.Maximized)
                .SelectMany(e => e.RichTextRibbonForm.RichEditControl.Observe()
                    .AssertRichEditControl(caller: $"{nameof(AssertShowInDocumentAction)} {item}")
                    .Buffer(e.RichTextRibbonForm.WhenEvent(nameof(e.RichTextRibbonForm.Activated)).DelayOnContext()).SelectMany().Take(1)
                    .Do(richEditControl => richEditControl.FindForm()?.Close())
                    .DelayOnContext()).Take(1).To<Frame>();

        public static IObservable<MapControl> AssertMapsControl(this DetailView detailView)
            => detailView.WhenControlViewItemControl().OfType<MapControl>().WhenNotDefault(control => control.Layers.Count)
                .SelectMany(control => control.Layers.OfType<ImageLayer>().ToNowObservable()
                    .SelectMany(layer => Observable.FromEventPattern(layer, nameof(layer.Error),ReactiveExtensions.ImmediateScheduler)
                        .SelectMany(pattern => ((MapErrorEventArgs)pattern.EventArgs).Exception.ThrowTestException().To<MapControl>())
                        .Merge(Observable.FromEventPattern(layer, nameof(layer.DataLoaded),ReactiveExtensions.ImmediateScheduler).To(control)).Take(1)))
                .Assert();
        
        public static IObservable<RichEditControl> AssertRichEditControl(this DetailView detailView, bool assertMailMerge = false)
            => detailView.WhenPropertyEditorControl().OfType<Control>().SelectControlRecursive().OfType<RichEditControl>()
                .AssertRichEditControl(assertMailMerge);
        
        public static IObservable<(ColumnView columnView, object value)> AssertGridControlDetailViewObjects(
            this IObservable<Frame> source, Func<ColumnView, int> count, [CallerMemberName] string caller = "")
            => source.WhenGridControlDetailViewObjects(count).Assert(o => $"{o} {caller}");
        
        public static IObservable<Frame> AssertDashboardViewGridControlDetailViewObjects(
            this IObservable<Frame> source, params string[] relationNames )
            => source.AssertDashboardViewGridControlDetailViewObjects(view => !relationNames.Contains(view.LevelName)
                ? throw new NotImplementedException(view.LevelName) : 1 );
        
        public static IObservable<Frame> AssertDashboardViewGridControlDetailViewObjects(
            this IObservable<Frame> source, Func<ColumnView, int> count, [CallerMemberName] string caller = "")
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .SelectMany(frame => frame.Observe().AssertGridControlDetailViewObjects(count,caller).IgnoreElements().ToFirst().To<Frame>())
                .Concat(source).ReplayFirstTake();
    }

    
}