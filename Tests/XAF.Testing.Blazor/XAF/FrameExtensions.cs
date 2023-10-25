using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.ReportsV2;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace XAF.Testing.Blazor.XAF{
    public static class FrameExtensions{
        public static IObservable<object> SelectDashboardColumnViewObject(this IObservable<DashboardViewItem> source){
            return source.Do(item => throw new NotImplementedException());
            // return source.SelectMany(item => item.InnerView.ToDetailView().WhenControlViewItemGridControl()
                // .Select(gridControl => gridControl.MainView).Cast<ColumnView>()
                // .SelectMany(gridView => gridView.ProcessEvent(EventType.Click)));
        }
        
        public static IObservable<object> WhenColumnViewObjects(this Frame frame,int count=0){
            return Observable.Empty<object>();
            // return frame.WhenGridControl().ToFirst().WhenObjects(count).Take(1);
        }
        
        public static IObservable<Frame> CreateNewObjectController(this Frame frame) 
            => frame.View.WhenObjectViewObjects(1).Take(1)
                .SelectMany(selectedObject => frame.ColumnViewCreateNewObject().SwitchIfEmpty(frame.ListViewCreateNewObject())
                    .SelectMany(newObjectFrame => newObjectFrame.View.ToCompositeView().CloneExistingObjectMembers(false, selectedObject)
                        .Select(_ => default(Frame)).IgnoreElements().Concat(newObjectFrame.YieldItem())));

        internal static IObservable<Frame> ColumnViewCreateNewObject(this Frame frame){
            // return frame.WhenGridControl().Select(t => t.frame).CreateNewObject();
            return Observable.Empty<Frame>();
        }


        public static IObservable<Frame> AssertReport(this Frame frame,[CallerMemberName]string caller="") 
            => frame.Application.WhenFrame("ReportViewer_DetailView")
                .Where(frame1 => ((IReportDataV2)frame1.View.CurrentObject).DisplayName==caller).To<Frame>()
                .SelectUntilViewClosed(frame1 => frame.Application.GetRequiredService<IReportResolver>().WhenResolved(frame1)
                    .SelectMany(report => report.WhenReady()).To(frame1).CloseWindow(frame))
                .Assert($"{nameof(AssertReport)}", caller: caller)
                .Select(frame1 => frame1)
                .To<Frame>();


        public static IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(this Frame frame){
            // return frame.WhenGridControl()
            //     .Publish(source => source
            //         .SelectMany(t => frame.Application.WhenFrame(((NestedFrame)t.frame)
            //                 .DashboardChildDetailView().ObjectTypeInfo.Type, ViewType.DetailView)
            //             .Where(frame1 => frame1.View.ObjectSpace.GetKeyValue(frame1.View.CurrentObject)
            //                 .Equals(((ColumnView)t.gridControl.MainView).FocusedRowObjectKey(frame1.View.ObjectSpace))))
            //         .Merge(Observable.Defer(() =>
            //             source.ToFirst().ProcessEvent(EventType.DoubleClick).To<Frame>().IgnoreElements())))
            //     .SwitchIfEmpty(frame.ProcessListViewSelectedItem())
            //     .Select(detailViewFrame => (frame: frame, detailViewFrame));
            return frame.ProcessListViewSelectedItem().Select(detailViewFrame => (frame, detailViewFrame));
        }
    }
}