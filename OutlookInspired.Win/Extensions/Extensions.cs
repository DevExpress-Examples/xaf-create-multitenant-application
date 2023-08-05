using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPivotGrid;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.Extensions{
    static class Extensions{
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