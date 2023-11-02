using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Office.Blazor.Controllers;
using XAF.Testing.XAF;

namespace XAF.Testing.Blazor.XAF{
    public static class ActionExtensions{
        public static IObservable<Frame> AssertShowInDocumentAction(this SingleChoiceAction action)
            => action.Application.WhenFrame(typeof(BlazorRichTextMailMergeObject),ViewType.DetailView)
                .SelectMany(frame => frame.View.ToDetailView().AssertRichEditControl().To(frame))
                .CloseWindow(action.Frame())
                .Assert();

    }
}