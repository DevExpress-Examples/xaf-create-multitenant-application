using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Office.Blazor.Components.Models;
using XAF.Testing.XAF;

namespace XAF.Testing.Blazor.XAF{
    public static class AssertExtensions{
        public static IObservable<DxRichEditModel> AssertRichEditControl(this IObservable<DxRichEditModel> source)
            => source.SelectMany(model => model.WhenCallback(editModel => editModel.DocumentLoaded)
                .Select(document => document).To(model)).Assert();
                
        public static IObservable<Unit> AssertViewItemControl<TControl>(this DetailView detailView,Func<TControl,IObservable<Unit>> readySignal)
            => detailView.WhenControlViewItemControl().OfType<TControl>()
                .SelectMany(readySignal)
                .Assert();

    }
}