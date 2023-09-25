using System.Runtime.InteropServices;
using System.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Map.Dashboard;
using DevExpress.Persistent.Base;
using DevExpress.Utils.Extensions;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraMap;
using DevExpress.XtraPivotGrid;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Services.Internal;
using IColorizer = DevExpress.XtraCharts.IColorizer;
using KeyColorColorizer = DevExpress.XtraMap.KeyColorColorizer;

namespace OutlookInspired.Win.Extensions.Internal{
    internal static class Extensions{
        public static void ProtectDetailViews(this XafApplication application, GridControl gridControl,Type objectType) 
            => gridControl.LevelTree.Nodes.ToArray()
                .Where(node => !application.CanRead(application.TypesInfo.FindTypeInfo(objectType).FindMember(node.RelationName).ListElementType))
                .Do(node => gridControl.LevelTree.Nodes.Remove(node))
                .Enumerate();

        public static ChartControl ApplyColors(this ChartControl chartControl,KeyColorColorizer colorizer){
            colorizer.Colors.Clear();
            colorizer.Colors.BeginUpdate();
            chartControl.GetPaletteEntries(20).ForEach(entry => colorizer.Colors.Add(entry.Color));
            colorizer.Colors.EndUpdate();
            chartControl.Series[0].View.Colorizer = (IColorizer)colorizer;
            return chartControl;
        }
        
        public static bool IsNotGroupedRow(this ColumnView columnView) 
            => columnView is not GridView view|| !view.IsGroupRow(columnView.FocusedRowHandle);
        public static bool IsNotInvalidRow(this ColumnView columnView) 
            => columnView.FocusedRowHandle!=GridControl.InvalidRowHandle;
        
        [DllImport("USER32.dll", CharSet = CharSet.Auto)]  
        static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        [SecuritySafeCritical]
        public static void LockRedraw(this Control control, Action action){
            SendMessage(control.Handle, 0x000B, 0, IntPtr.Zero);
            action();
            SendMessage(control.Handle, 0x000B, 1, IntPtr.Zero);
        }

        public static void To(this IZoomToRegionService zoomService, GeoPoint pointA, GeoPoint pointB, double margin = 0.2){
            if (pointA == null || pointB == null || zoomService == null) return;
            var (latDiff, longDiff) = (pointB.Latitude - pointA.Latitude, pointB.Longitude - pointA.Longitude);
            var (latPad, longPad) = (margin.CalculatePadding(latDiff), margin.CalculatePadding(longDiff));
            zoomService.ZoomToRegion(new GeoPoint(pointA.Latitude - latPad, pointA.Longitude - longPad),
                new GeoPoint(pointB.Latitude + latPad, pointB.Longitude + longPad),
                new GeoPoint((pointA.Latitude + pointB.Latitude) / 2, (pointA.Longitude + pointB.Longitude) / 2));
        }
        
        public static void To(this IZoomToRegionService zoomService, IEnumerable<IMapsMarker> mapsMarkers, double margin = 0.25){
            var points = mapsMarkers.Select(m => m.ToGeoPoint()).Where(p => p != null && !Equals(p, new GeoPoint(0, 0))).ToList();
            if (!points.Any()) return;
            zoomService.To(new GeoPoint(points.Min(p => p.Latitude), points.Min(p => p.Longitude)),
                new GeoPoint(points.Max(p => p.Latitude), points.Max(p => p.Longitude)), margin);
        }
        
        static double CalculatePadding(this double margin,double delta) 
            => delta > 0 ? Math.Max(0.1, delta * margin) : delta < 0 ? Math.Min(-0.1, delta * margin) : 0;

        public static GeoPoint ToGeoPoint(this IMapsMarker mapsMarker) 
            => new(mapsMarker.Latitude, mapsMarker.Longitude);
        
        public static object FocusedRowObject(this ColumnView columnView, IObjectSpace objectSpace,Type objectType){
            if (columnView.FocusedRowObject == null || !columnView.IsServerMode)
                return columnView.FocusedRowObject;
            else if (columnView.IsNotGroupedRow()&&columnView.IsNotInvalidRow())
                return objectSpace.GetObjectByKey(objectType, columnView.FocusedRowObject);
            else
                return null;
        }

        public static Dictionary<PivotGridField, RepositoryItem> AddRepositoryItems(this PivotGridControl pivotGridControl,ListView view) 
            => view.Model.Columns.Where(column => column.Index>=0)
                .Select(column => {
                    var pivotGridField = pivotGridControl.Fields[column.ModelMember.Name];
                    return pivotGridField != null && typeof(IInplaceEditSupport).IsAssignableFrom(column.PropertyEditorType)
                        ? (pivotGridField, repositoryItem: ((IInplaceEditSupport)column.NewPropertyEditor()).CreateRepositoryItem()) : default;
                })
                .WhereNotDefault()
                .Do(t => pivotGridControl.RepositoryItems.Add(t.repositoryItem))
                .ToDictionary(t => t.pivotGridField, t => t.repositoryItem);
        
        public static ColumnView ColumnView(this Control userControl) 
            => (ColumnView)userControl.Controls.OfType<GridControl>().First().MainView;

        public static T GridView<T>(this ListView listView) where T:GridView
            => (listView.Editor.Control as GridControl)?.MainView as T;
        
        public static void IncreaseFontSize(this GridView gridView, ITypeInfo typeInfo){
            var columns = typeInfo.AttributedMembers<FontSizeDeltaAttribute>().ToDictionary(
                attribute => gridView.Columns[attribute.memberInfo.BindingName].VisibleIndex,
                attribute => attribute.attribute.Delta);
            gridView.CustomDrawCell += (_, e) => {
                if (columns.TryGetValue(e.Column.VisibleIndex, out var column)) e.DrawCell( column);
            };
        }

        private static void DrawCell(this RowCellCustomDrawEventArgs e, int fontSizeDelta){
            e.Appearance.FillRectangle(e.Cache, e.Bounds);
            e.Appearance.FontSizeDelta = fontSizeDelta;
            e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
            e.Handled = true;
        }
    }
}