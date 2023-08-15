using System.Collections;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Base.Handler;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.Handler;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraGrid.Views.Layout.Handler;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Xml;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class WinComponentExtensions{
        public static void AddNewRow(this GridView gridView,params (string fieldName,object value)[] values){
            gridView.AddNewRow();
            gridView.FocusedRowHandle = GridControl.NewItemRowHandle;
            values.Do(t => gridView.SetRowCellValue(gridView.FocusedRowHandle, t.fieldName, t.value)).Enumerate();
            gridView.UpdateCurrentRow();
        } 
        public static string YearlyOnWorkDayRecurrenceInfoXml(this DateTime now) 
            => new RecurrenceInfoXmlPersistenceHelper(new RecurrenceInfo(now){
                Type = RecurrenceType.Yearly, Periodicity = 1, Month = now.Month, WeekOfMonth = WeekOfMonth.First,
                Duration = TimeSpan.FromHours(1), WeekDays = WeekDays.WorkDays
            }).ToXml();


        public static IObservable<object> WhenDataSourceChanged(this GridControl gridControl) 
            => gridControl.WhenEvent(nameof(GridControl.DataSourceChanged));

        public static IObservable<object> GridDetailViewObjects(this GridView view) 
            => view.WhenEvent<CustomMasterRowEventArgs>(nameof(GridView.MasterRowExpanded))
                .Select(e => view.GetDetailView(e.RowHandle,e.RelationIndex)).Cast<ColumnView>()
                .Delay(100.Milliseconds(),new SynchronizationContextScheduler(SynchronizationContext.Current!))
                .SelectMany(baseView => ((IEnumerable)baseView.DataSource).Cast<object>().Take(1).ToArray())
                .Take(view.GridControl.LevelTree.Nodes.Count).BufferUntilCompleted().SelectMany()
                .MergeToObject(view.Observe().Do(gridView => gridView.RecursiveExpandAndFocus(0)).IgnoreElements());

        public static void RecursiveExpandAndFocus(this GridView masterView, int masterRowHandle){
            var relationCount = masterView.GetRelationCount(masterRowHandle);
            for (var index = 0; index < relationCount; index++){
                masterView.ExpandMasterRow(masterRowHandle, index);
                if (masterView.GetDetailView(masterRowHandle, index) is GridView childView){
                    childView.FocusedRowHandle = 0;
                    for (var handle = 0; handle < childView.DataRowCount; handle++)
                        RecursiveExpandAndFocus(childView, handle);
                }
            }
        }
        public static IObservable<ColumnView> ProcessEvent(this IObservable<GridControl> source,global::DevExpress.Utils.Controls.EventType eventType)
            => source.Select(control => control.MainView).Cast<ColumnView>()
                .SelectMany(layoutView => layoutView.ProcessEvent(eventType));

        public static IObservable<ColumnView> ProcessEvent(this ColumnView columnView,global::DevExpress.Utils.Controls.EventType eventType) 
            => columnView.WhenEvent<FocusedRowObjectChangedEventArgs>(nameof(columnView.FocusedRowObjectChanged))
                .WhenNotDefault(e => e.Row)
                .Do(_ => columnView.ViewHandler().ProcessEvent(eventType, EventArgs.Empty))
                .To(columnView)
                .Merge(Observable.Defer(() => {
                    var row = columnView.FindRow(((IEnumerable)columnView.DataSource).Cast<object>().First());
                    columnView.Focus();
                    columnView.SelectRow(row);
                    columnView.SelectRow(2);
                    columnView.FocusedRowHandle = 2;
                    return Observable.Empty<ColumnView>();
                }).To<ColumnView>());

        private static BaseViewHandler ViewHandler(this ColumnView columnView) 
            => columnView is LayoutView layoutView ? new LayoutViewHandler(layoutView) : new GridHandler((GridView)columnView);

        public static IObservable<object> HasObjects(this IObservable<GridControl> source)
            => source.Select(control => control.DataSource).Cast<IEnumerable>()
                .WhenNotDefault().SelectMany(enumerable => enumerable.Cast<object>());



    }
}