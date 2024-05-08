using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Office.Blazor.Components.Models;
using DevExpress.Persistent.BaseImpl.EF;
using XAF.Testing.XAF;

namespace XAF.Testing.Blazor.XAF{
    public static class AssertExtensions{
        public static IObservable<Unit> AssertReports(this BlazorApplication application,IObservable<Unit> canNavigate) 
            => application.AssertReports(AssertDesigner,canNavigate);
        
        private static IObservable<Unit> AssertDesigner(this SimpleAction showReportDesigner) 
            => showReportDesigner.Application.MainWindow.WhenViewChanged().Take(1).DelayOnContext()
                .Select(window => window.View.CurrentObject).Cast<ReportDataV2>()
                .SelectMany(v2 => v2.LoadReport().Observe().Validate().DelayOnContext().To(showReportDesigner)
                    .SelectMany(_ => showReportDesigner.Application.NavigateBack())
                    .SelectMany(frame => showReportDesigner.Application.MainWindow.View.WhenControlsCreated(true).To(frame))
                    .SelectMany(_ => showReportDesigner.Application.MainWindow.View.ToListView()
                        .SelectObject(showReportDesigner.Application.MainWindow.View.ObjectSpace.GetObject(v2)))).ToUnit();

        public static IObservable<DxRichEditModel> AssertRichEditControl(this IObservable<DxRichEditModel> source)
            => source.SelectMany(model => model.WhenCallback(editModel => editModel.DocumentLoaded)
                .Select(document => document).To(model)).Assert();
                
        public static IObservable<Unit> AssertViewItemControl<TControl>(this DetailView detailView,Func<TControl,IObservable<Unit>> readySignal)
            => detailView.WhenControlViewItemControl().OfType<TControl>()
                .SelectMany(readySignal)
                .Assert();

    }
}