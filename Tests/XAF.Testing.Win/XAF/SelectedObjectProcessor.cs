using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.Utils.Controls;
using DevExpress.XtraGrid.Views.Base;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace XAF.Testing.Win.XAF{
    public class SelectedObjectProcessor : ISelectedObjectProcessor{
        public IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(Frame listViewFrame) 
            => listViewFrame.WhenGridControl()
                .Publish(source => source.SelectMany(t => listViewFrame.Application.WhenFrame(((NestedFrame)t.frame)
                            .DashboardChildDetailView().ObjectTypeInfo.Type, ViewType.DetailView)
                        .Where(frame1 => frame1.View.ObjectSpace.GetKeyValue(frame1.View.CurrentObject)
                            .Equals(((ColumnView)t.gridControl.MainView).FocusedRowObjectKey(frame1.View.ObjectSpace))))
                    .Merge(Observable.Defer(() =>
                        source.ToFirst().ProcessEvent(EventType.DoubleClick).To<Frame>().IgnoreElements())))
                .SwitchIfEmpty(listViewFrame.ProcessListViewSelectedItem())
                .Select(detailViewFrame => (frame: listViewFrame, detailViewFrame));
    }
}