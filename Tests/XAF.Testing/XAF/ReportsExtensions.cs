using System.Reactive.Linq;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using XAF.Testing.RX;

namespace XAF.Testing.XAF{
    public static class ReportsExtensions{
        public static IObservable<XtraReport> WhenReady(this XtraReport report) 
            => report.PrintingSystem
                .WhenEvent<ExceptionEventArgs>(nameof(PrintingSystemBase.CreateDocumentException))
                .Select(e => e.Exception).Buffer(report.WhenEvent(nameof(XRControl.AfterPrint))).Take(1)
                .ObserveOnContext()
                .Select(exceptions => (exceptions: exceptions.Count, pages: report.Pages.Count))
                .WhenDefault(t => t.exceptions).WhenNotDefault(t => t.pages).To(report);
    }
}