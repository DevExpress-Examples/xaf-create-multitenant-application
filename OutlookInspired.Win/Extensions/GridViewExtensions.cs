using DevExpress.ExpressApp.DC;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.Extensions{
    static class GridViewExtensions{
        // public static void Configure(this LayoutView layoutView,LayoutViewColumn colPhoto,LayoutViewColumn colAddress,LayoutViewColumn colFullName,LayoutViewColumn colEmail,LayoutViewColumn colPhone){
        //     layoutView.OptionsBehavior.Editable = false;
        //     layoutView.OptionsFind.AlwaysVisible = true;
        //     layoutView.OptionsFind.ShowFindButton = false;
        //     layoutView.OptionsFind.ShowSearchNavButtons = false;
        //     layoutView.OptionsHeaderPanel.EnableCarouselModeButton = false;
        //     layoutView.OptionsHeaderPanel.EnableColumnModeButton = false;
        //     layoutView.OptionsHeaderPanel.EnableCustomizeButton = false;
        //     layoutView.OptionsHeaderPanel.EnableMultiColumnModeButton = false;
        //     layoutView.OptionsHeaderPanel.EnableMultiRowModeButton = false;
        //     layoutView.OptionsHeaderPanel.EnablePanButton = false;
        //     layoutView.OptionsHeaderPanel.EnableRowModeButton = false;
        //     layoutView.OptionsHeaderPanel.EnableSingleModeButton = false;
        //     layoutView.OptionsHeaderPanel.ShowCarouselModeButton = false;
        //     layoutView.OptionsHeaderPanel.ShowColumnModeButton = false;
        //     layoutView.OptionsHeaderPanel.ShowCustomizeButton = false;
        //     layoutView.OptionsView.ShowCardCaption = false;
        //     layoutView.OptionsView.ShowHeaderPanel = false;
        //     
        //     colPhoto.ConfigureColumn($"{nameof(Employee.Picture)}.{nameof(Picture.Data)}");
        //     colPhoto.AppearanceCell.Options.UseTextOptions = true;
        //     colPhoto.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
        //     colAddress.ConfigureColumn(nameof(Employee.Address));
        //     colFullName.ConfigureColumn(nameof(Employee.FullName));
        //     colEmail.ConfigureColumn(nameof(Employee.Email));
        //     colPhone.ConfigureColumn(nameof(Employee.HomePhone));
        // }
        
        // private static void ConfigureColumn(this LayoutViewColumn layoutViewColumn,string fieldName){
        //     layoutViewColumn.AppearanceCell.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        //     layoutViewColumn.AppearanceCell.Options.UseFont = true;
        //     layoutViewColumn.FieldName = fieldName;
        //     layoutViewColumn.OptionsColumn.AllowEdit = false;
        //     layoutViewColumn.OptionsColumn.AllowFocus = false;
        // }

        // public static void SetRelationName(this BaseView gridView,string relationName) 
            // => gridView.GridControl.LevelTree.Nodes.First(node => node.LevelTemplate == gridView).RelationName = relationName;

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