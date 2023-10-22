using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Office.Blazor.Controllers;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace XAF.Testing.Blazor.XAF{
    public static class ActionExtensions{
        public static IObservable<Frame> AssertShowInDocumentAction(this SingleChoiceAction action)
            => action.Application.WhenFrame(typeof(BlazorRichTextMailMergeObject),ViewType.DetailView)
                .SelectMany(frame => frame.View.ToDetailView().AssertRichEditControl().To(frame))
                .CloseWindow(action.Frame())
                .Assert();
        // => action.Frame().GetController<RichTextServiceController>()
        //     .WhenEvent<CustomRichTextRibbonFormEventArgs>(nameof(RichTextServiceController.CustomCreateRichTextRibbonForm)).Take(1)
        //     .Do(e => e.RichTextRibbonForm.WindowState = FormWindowState.Maximized)
        //     .SelectMany(e => e.RichTextRibbonForm.RichEditControl.Observe()
        //         .AssertRichEditControl(caller: $"{nameof(AssertShowInDocumentAction)} {item}")
        //         .Buffer(e.RichTextRibbonForm.WhenEvent(nameof(e.RichTextRibbonForm.Activated)).DelayOnContext()).SelectMany().Take(1)
        //         .Do(richEditControl => richEditControl.FindForm()?.Close())
        //         .DelayOnContext()).Take(1).To<Frame>();

    }
}