using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Map.Dashboard;
using DevExpress.Persistent.Base;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraMap;
using DevExpress.XtraPivotGrid;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.Extensions{
    public static class Extensions{
        public static void ZoomTo(this IZoomToRegionService zoomService, GeoPoint pointA, GeoPoint pointB, double margin = 0.2){
            if(pointA == null || pointB == null || zoomService == null) return;
            var latPadding = CalculatePadding(pointB.Latitude - pointA.Latitude, margin);
            var longPadding = CalculatePadding(pointB.Longitude - pointA.Longitude, margin);
            zoomService.ZoomToRegion(
                new GeoPoint(pointA.Latitude - latPadding, pointA.Longitude - longPadding),
                new GeoPoint(pointB.Latitude + latPadding, pointB.Longitude + longPadding),
                new GeoPoint(0.5 * (pointA.Latitude + pointB.Latitude), 0.5 * (pointA.Longitude + pointB.Longitude)));

        }
        
        public static void ZoomTo(this IZoomToRegionService zoomService, IEnumerable<IMapsMarker> mapsMarkers, double margin = 0.25) {
            GeoPoint ptA = null;
            GeoPoint ptB = null;
            foreach(var address in mapsMarkers) {
                if(ptA == null) {
                    ptA = address.ToGeoPoint();
                    ptB = address.ToGeoPoint();
                    continue;
                }
                GeoPoint pt = address.ToGeoPoint();
                if(pt == null || Equals(pt, new GeoPoint(0, 0)))
                    continue;
                ptA.Latitude = Math.Min(ptA.Latitude, pt.Latitude);
                ptA.Longitude = Math.Min(ptA.Longitude, pt.Longitude);
                ptB.Latitude = Math.Max(ptB.Latitude, pt.Latitude);
                ptB.Longitude = Math.Max(ptB.Longitude, pt.Longitude);
            }
            zoomService.ZoomTo( ptA, ptB, margin);
        }

        
        static double CalculatePadding(double delta, double margin) 
            => delta > 0 ? Math.Max(0.1, delta * margin) : delta < 0 ? Math.Min(-0.1, delta * margin) : 0;

        public static GeoPoint ToGeoPoint(this IMapsMarker mapsMarker) 
            => new(mapsMarker.Latitude, mapsMarker.Longitude);
        
        public static object FocusedRowObject(this ColumnView columnView, IObjectSpace objectSpace,Type objectType) 
            => columnView.FocusedRowObject == null || !columnView.IsServerMode ? columnView.FocusedRowObject
                : objectSpace.GetObjectByKey(objectType, columnView.FocusedRowObject);

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