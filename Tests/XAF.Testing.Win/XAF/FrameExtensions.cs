using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Office.Win;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace XAF.Testing.Win.XAF{
    public static class FrameExtensions{
        public static void AddNewRowAndCloneMembers(this Frame frame, object existingObject){
            var valueTuples = frame.View.ToCompositeView().CloneExistingObjectMembers(true, existingObject).ToArray();
            ((GridListEditor)frame.View.ToListView().Editor).GridView.AddNewRow(valueTuples);
        }

        public static IObservable<Frame> CreateNewObjectController(this Frame frame) 
            => frame.View.WhenObjectViewObjects(1).Take(1)
                .SelectMany(selectedObject => frame.ColumnViewCreateNewObject().SwitchIfEmpty(frame.ListViewCreateNewObject())
                    .SelectMany(newObjectFrame => newObjectFrame.View.ToCompositeView().CloneExistingObjectMembers(false, selectedObject)
                        .Select(_ => default(Frame)).IgnoreElements().Concat(newObjectFrame.YieldItem())));

        public static IObservable<Window> WhenMaximized(this Window window) 
            => window.Observe().Do(_ => ((Form)window.Template).WindowState = FormWindowState.Maximized)
                .DelayOnContext();
        public static IObservable<Window> CloseWindow(this IObservable<Frame> source) 
            => source.Cast<Window>().DelayOnContext().Do(frame => frame.Close()).DelayOnContext().IgnoreElements();
        public static IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(this Frame frame)
            => frame.WhenGridControl()
                .Publish(source => source
                    .SelectMany(t => frame.Application.WhenFrame(((NestedFrame)t.frame)
                            .DashboardChildDetailView().ObjectTypeInfo.Type, ViewType.DetailView)
                        .Where(frame1 => frame1.View.ObjectSpace.GetKeyValue(frame1.View.CurrentObject)
                            .Equals(((ColumnView)t.gridControl.MainView).FocusedRowObjectKey(frame1.View.ObjectSpace))))
                    .Merge(Observable.Defer(() =>
                        source.ToFirst().ProcessEvent(EventType.DoubleClick).To<Frame>().IgnoreElements())))
                .SwitchIfEmpty(frame.ProcessListViewSelectedItem())
                .Select(detailViewFrame => (frame: frame, detailViewFrame));

        public static IObservable<object> WhenColumnViewObjects(this Frame frame,int count=0) 
            => frame.WhenGridControl().ToFirst().WhenObjects(count).Take(1);

        public static IObservable<(ColumnView columnView, object value)> WhenGridViewDetailViewObjects(this Frame frame,Func<ColumnView,int> count=null)
            => frame.WhenGridControl().ToFirst().Take(1).Select(control => control.MainView).OfType<GridView>()
                .SelectMany(view => view.WhenGridDetailViewObjects(count));

        public static IObservable<(GridControl gridControl, Frame frame)> WhenGridControl(this Frame frame) 
            => (frame.View is DashboardView ? frame.DashboardViewItems(ViewType.DetailView)
                    .Where(item => item.Model.ActionsToolbarVisibility != ActionsToolbarVisibility.Hide)
                    .ToNowObservable().ToFrame() : frame.Observe())
                .If(frame1 => frame1.View is DetailView,frame1 => frame1.View.ToDetailView().WhenControlViewItemGridControl()
                    .Select(gridControl => (gridControl, frame1)));
        
        public static IObservable<(ColumnView columnView, object value)> WhenGridControlDetailViewObjects(this IObservable<Frame> source,Func<ColumnView,int> count=null) 
            => source.SelectMany(frame => frame.WhenGridViewDetailViewObjects(count));

        public static IObservable<ColumnView> SelectDashboardColumnViewObject(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenControlViewItemGridControl()
                .Select(gridControl => gridControl.MainView).Cast<ColumnView>()
                .SelectMany(gridView => gridView.ProcessEvent(EventType.Click)));

        internal static IObservable<Frame> ColumnViewCreateNewObject(this Frame frame)
            => frame.WhenGridControl().Select(t => t.frame).CreateNewObject();
        
        public static IObservable<(GridControl gridControl, Frame frame)> WhenGridControl(this IObservable<Frame> source) 
            => source.OfView<DetailView>().SelectMany(frame =>
                frame.View.ToDetailView().WhenControlViewItemGridControl().Select(gridControl => (gridControl, frame)));
    }
}