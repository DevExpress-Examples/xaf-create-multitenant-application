using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ReportsV2;
using XAF.Testing.XAF;

namespace XAF.Testing.Blazor.XAF{
    public static class FrameExtensions{
        public static IObservable<Frame> AssertReport(this Frame frame,[CallerMemberName]string caller="") 
            => frame.Application.WhenFrame("ReportViewer_DetailView")
                .Where(frame1 => ((IReportDataV2)frame1.View.CurrentObject).DisplayName==caller).To<Frame>()
                .SelectUntilViewClosed(frame1 => frame.Application.GetRequiredService<IReportResolver>().WhenResolved(frame1)
                    .SelectMany(report => report.WhenReady()).To(frame1).CloseWindow(frame))
                .Assert($"{nameof(AssertReport)}", caller: caller)
                .Select(frame1 => frame1)
                .To<Frame>();


        public static IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(this Frame frame) 
            => frame.View.Observe().OfType<DetailView>()
                .SelectMany(detailView => detailView.WhenGridControl()
                    .SelectMany(gridControl => frame.Application.GetRequiredService<IUserControlProcessSelectedObject>()
                        .Process(frame, gridControl)))
                .SwitchIfEmpty(frame.ProcessListViewSelectedItem())
                .Select(detailViewFrame => (frame,detailViewFrame));
    }
}