using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Office.Win;
using DevExpress.ExpressApp.ReportsV2.Win;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraMap;
using DevExpress.XtraPdfViewer;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using ListView = DevExpress.ExpressApp.ListView;
using TabbedGroup = DevExpress.XtraLayout.TabbedGroup;
using View = DevExpress.ExpressApp.View;

namespace XAF.Testing.Win.XAF{
    public static class AssertExtensions{
        public static IObservable<Frame> AssertDashboardViewShowInDocumentAction(this IObservable<Frame> source,Func<SingleChoiceAction,int> itemsCount) 
            => source.DashboardViewItem(item => item.MasterViewItem()).ToFrame()
                .AssertSingleChoiceAction("ShowInDocument",itemsCount)
                .SelectMany(action => action.Items.ToNowObservable()
                    .SelectManySequential(item => action.Trigger(action.Frame().GetController<RichTextServiceController>()
                        .WhenEvent<CustomRichTextRibbonFormEventArgs>(nameof(RichTextServiceController.CustomCreateRichTextRibbonForm)).Take(1)
                        .Do(e => e.RichTextRibbonForm.WindowState=FormWindowState.Maximized)
                        .SelectMany(e => e.RichTextRibbonForm.RichEditControl.Observe().AssertRichEditControl(caller:$"{nameof(AssertShowInDocumentAction)} {item}")
                            .Buffer(e.RichTextRibbonForm.WhenEvent(nameof(e.RichTextRibbonForm.Activated)).DelayOnContext()).SelectMany().Take(1)
                            .Do(richEditControl => richEditControl.FindForm()?.Close())
                            .DelayOnContext()).Take(1).To<Frame>(),() => item)))
                .IgnoreElements().Concat(source).ReplayFirstTake();

        
        
        
        public static IObservable<Frame> AssertReport(this Frame frame, [CallerMemberName]string caller="") 
            => frame.GetController<WinReportServiceController>()
                .WhenEvent<WinReportServiceController.CustomizePrintToolEventArgs>(
                    nameof(WinReportServiceController.CustomizePrintTool)).Take(1)
                .SelectMany(e => e.PrintTool.PrintingSystem
                    .WhenEvent(nameof(e.PrintTool.PrintingSystem.CreateDocumentException))
                    .Select(pattern => ((ExceptionEventArgs)pattern.EventArgs).Exception)
                    .Buffer(((XtraReport)e.PrintTool.Report).WhenEvent(nameof(XtraReport.AfterPrint))).Take(1)
                    .Select(exceptions => (exceptions: exceptions.Count, pages: ((XtraReport)e.PrintTool.Report).Pages.Count))
                    .DelayOnContext()
                    .Do(_ => e.PrintTool.PreviewRibbonForm.Close())
                    .WhenDefault(t => t.exceptions)
                    .WhenNotDefault(t => t.pages).Select(_ => default(Frame))
                    .DelayOnContext()).Assert($"{nameof(AssertReport)}",caller:caller);

        public static IObservable<Frame> AssertExistingObjectDetailView(this XafApplication application,Type objectType=null) 
            => application.AssertExistingObjectDetailView(_ => Observable.Empty<Unit>(),objectType);

        

        public static IObservable<object> AssertObjectsCount(this View view, int objectsCount) 
            => view.WhenDataSourceChanged().FirstAsync(o => o is CollectionSourceBase collectionSourceBase
                    ? collectionSourceBase.GetCount() == objectsCount : ((GridControl)o).MainView.DataRowCount == objectsCount)
                .Assert($"{nameof(AssertObjectsCount)} {view.Id}");

        public static IObservable<object> WhenDataSourceChanged(this View view) 
            => view is ListView listView ? listView.CollectionSource.WhenCriteriaApplied()
                : view.ToDetailView().WhenControlViewItemGridControl().SelectMany(control => control.WhenDataSourceChanged().To(control));

        

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
                    .SelectMany(layer => Observable.FromEventPattern(layer, nameof(layer.Error),EventExtensions.ImmediateScheduler)
                        .SelectMany(pattern => ((MapErrorEventArgs)pattern.EventArgs).Exception.ThrowTestException().To<MapControl>())
                        .Merge(Observable.FromEventPattern(layer, nameof(layer.DataLoaded),EventExtensions.ImmediateScheduler).To(control)).Take(1)))
                .Assert();
        
        public static IObservable<RichEditControl> AssertRichEditControl(this DetailView detailView, bool assertMailMerge = false)
            => detailView.WhenPropertyEditorControl().OfType<Control>().SelectControlRecursive().OfType<RichEditControl>()
                .AssertRichEditControl(assertMailMerge);

        
        
        public static IObservable<Frame> AssertSelectListViewObject(this IObservable<Frame> source,params object[] objects)
            => source.ToListView().SelectMany(listView => objects.FirstOrDefault() is{ } selectedObject ? listView.SelectObject(selectedObject)
                    : listView.Objects().Take(1).SelectMany(o => listView.SelectObject(o))).Assert().IgnoreElements().To<Frame>()
                .Concat(source);
        
        public static IObservable<TabbedGroup> AssertTabbedGroup(this XafApplication application,
            Type objectType = null, int tabPagesCount = 0,Func<DetailView,bool> match=null,[CallerMemberName]string caller="")
            => application.AssertTabControl<TabbedGroup>(objectType,match)
                .If(group => tabPagesCount > 0 && group.TabPages.Count != tabPagesCount,group => group.Observe().DelayOnContext()
                    .SelectMany(_ => new Exception(
                        $"{nameof(AssertTabbedGroup)} {objectType?.Name} expected {tabPagesCount} but was {group.TabPages.Count}").ThrowTestException().To<TabbedGroup>()),
                    group => group.Observe());
        

        public static IObservable<object> AssertWindowHasObjects(this IObservable<Frame> source)
            => source.If(window => window.DashboardViewItems<DetailView>().Any(),window => window.Observe().WhenObjects().ToSecond()
                    .Assert($"{nameof(AssertWindowHasObjects)} {window.View.Id}"),
                window => window.DashboardViewItems<ListView>().ToNowObservable().BufferUntilCompleted()
                    .SelectMany(listViews => listViews.ToNowObservable().SelectMany(listView => listView.WhenObjects().Take(1))
                        .Take(listViews.Length))
                    .Assert($"{nameof(AssertWindowHasObjects)} {window.View.Id}"));

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

        public static IObservable<Unit> AssertDetailViewGridControlHasObjects(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenControlViewItemGridControl().WhenObjects()).ToUnit().Assert();
        
        public static IObservable<Unit> AssertPdfViewer(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenViewItemWinControl<PropertyEditor>(typeof(PdfViewer)))
                .SelectMany(t => t.item.WhenEvent(nameof(PropertyEditor.ValueRead))
                    .WhenNotDefault(_ => ((PdfViewer)t.control).PageCount))
                .ToUnit().Assert();

        public static IObservable<Frame> AssertNestedListView(this IObservable<TabbedGroup> source, Frame frame, Type objectType, int selectedTabPageIndex,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null, Func<Frame,AssertAction> assert=null,bool inlineEdit=false,[CallerMemberName]string caller="")
            => source.AssertNestedListView(frame, objectType, group => group.SelectedTabPageIndex=selectedTabPageIndex,existingObjectDetailview,assert,inlineEdit,caller);
        
        public static IObservable<Frame> AssertNestedListView(this IObservable<TabbedGroup> source, Frame frame, Type objectType, Action<TabbedGroup> tabGroupAction,
            Func<Frame, IObservable<Unit>> existingObjectDetailview = null, Func<Frame,AssertAction> assert = null,bool inlineEdit=false,[CallerMemberName]string caller="") 
            => frame.AssertNestedListView(objectType,existingObjectDetailview,assert,inlineEdit,caller)
                .Merge(source.DelayOnContext().Do(tabGroupAction).DelayOnContext().IgnoreElements().To<Frame>());
    }

    
}