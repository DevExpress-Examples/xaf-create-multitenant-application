using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.Utils.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace XAF.Testing.Win.XAF{
    public static class FrameExtensions{
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