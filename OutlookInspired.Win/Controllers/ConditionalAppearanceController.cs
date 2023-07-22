using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using OutlookInspired.Module;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.Controllers{
    public class FontSizeDeltaAttributeListViewController:ListViewControllerBase{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if ((View.Editor.Control as GridControl)?.MainView is AdvBandedGridView advBandedGridView){
                var attributes = View.ObjectTypeInfo.AttributedMembers<FontSizeDeltaAttribute>().ToArray();
                var columns = FontSizeDelta(attributes, advBandedGridView);
                advBandedGridView.CustomDrawCell+= (_, e) => {
                    if (columns.Contains(e.Column.VisibleIndex)){
                        e.Appearance.FillRectangle(e.Cache, e.Bounds);
                        var r = e.Bounds;
                        var inflationValue = -1 * e.RowHandle * 2;
                        r.Inflate(0, inflationValue); 
                        e.Appearance.DrawString(e.Cache, e.DisplayText, r);
                        e.Handled = true;
                    }
                };
            }
        }

        private static int[] FontSizeDelta((FontSizeDeltaAttribute attribute, IMemberInfo memberInfo)[] attributes, AdvBandedGridView advBandedGridView) 
            => attributes.Do(attribute => advBandedGridView.Columns[attribute.memberInfo.BindingName].AppearanceCell.FontSizeDelta = attribute.attribute.Delta)
                .Select(attribute => {
                    var column = advBandedGridView.Columns[attribute.memberInfo.BindingName];
                    column.AppearanceCell.FontSizeDelta = attribute.attribute.Delta;
                    return column.VisibleIndex;
                }).Distinct().ToArray();
    }
}