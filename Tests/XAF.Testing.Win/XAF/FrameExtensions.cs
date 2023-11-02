using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using XAF.Testing.XAF;

namespace XAF.Testing.Win.XAF{
    public static class FrameExtensions{
        public static void AddNewRowAndCloneMembers(this Frame frame, object existingObject){
            var valueTuples = frame.View.ToCompositeView().CloneExistingObjectMembers(true, existingObject).ToArray();
            ((GridListEditor)frame.View.ToListView().Editor).GridView.AddNewRow(valueTuples);
        }

        public static IObservable<Window> WhenMaximized(this Window window) 
            => window.Observe().Do(_ => ((Form)window.Template).WindowState = FormWindowState.Maximized)
                .DelayOnContext();
        public static IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(this Frame frame) 
            => frame.View.Observe().OfType<DetailView>()
                .SelectMany(detailView => detailView.WhenGridControl()
                    .SelectMany(gridControl => {
                        var focusedRowObjectKey = ((ColumnView)gridControl.MainView).FocusedRowObjectKey(detailView.ObjectSpace);
                        return frame.Application.WhenFrame(((NestedFrame)frame).DashboardChildDetailView().ObjectTypeInfo.Type, ViewType.DetailView)
                            .Where(frame1 => frame1.View.ObjectSpace.GetKeyValue(frame1.View.CurrentObject).Equals(focusedRowObjectKey))
                            .Merge(Observable.Defer(() => gridControl.ProcessEvent(EventType.DoubleClick).To<Frame>().IgnoreElements()));
                    }))
                .SwitchIfEmpty(frame.ProcessListViewSelectedItem())
                .Select(detailViewFrame => (frame,detailViewFrame));

        public static IObservable<object> WhenColumnViewObjects(this Frame frame,int count=0) 
            => frame.View.Observe().OfType<DetailView>().SelectMany(detailView => detailView.WhenGridControl().WhenObjects(count).Take(1));

        public static IObservable<(ColumnView columnView, object value)> WhenGridViewDetailViewObjects(this Frame frame,Func<ColumnView,int> count=null)
            => frame.View.ToDetailView().WhenGridControl().Take(1).Select(control => control.MainView).OfType<GridView>()
                .SelectMany(view => view.WhenGridDetailViewObjects(count));
        
        public static IObservable<(ColumnView columnView, object value)> WhenGridControlDetailViewObjects(this IObservable<Frame> source,Func<ColumnView,int> count=null) 
            => source.SelectMany(frame => frame.WhenGridViewDetailViewObjects(count));

        public static IObservable<ColumnView> SelectDashboardColumnViewObject(this IObservable<DashboardViewItem> source)
            => source.SelectMany(item => item.InnerView.ToDetailView().WhenControlViewItemGridControl()
                .Select(gridControl => gridControl.MainView).Cast<ColumnView>()
                .SelectMany(gridView => gridView.ProcessEvent(EventType.Click))
                .Select(view => view));
    }
}