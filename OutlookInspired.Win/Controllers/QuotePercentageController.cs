using DevExpress.ExpressApp;
using DevExpress.ExpressApp.PivotGrid.Win;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPivotGrid;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Win.Editors;

namespace OutlookInspired.Win.Controllers{
    public class QuotePercentageController:ObjectViewController<ListView,Quote>{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            var pivotGridListEditor = ((PivotGridListEditor)View.Editor);
            var pivotGridControl = pivotGridListEditor.PivotGridControl;
            var repositoryItemProgressBar = new RepositoryItemProgressBar(){
                PercentView = true,ShowTitle = true,
                DisplayFormat = { FormatType = FormatType.Numeric,FormatString = "{0}%"}
            };
            repositoryItemProgressBar.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            pivotGridControl.RepositoryItems.Add(repositoryItemProgressBar);
            var field = pivotGridControl.Fields[nameof(Quote.Opportunity)];
            pivotGridControl.CustomCellEdit += (o, e) => {
                if (e.DataField == field){
                    e.RepositoryItem = repositoryItemProgressBar;
                }
            };
            pivotGridControl.CustomCellValue += (o, e) => {
                if (e.DataField == field){
                    e.Value = Convert.ToDecimal(e.Value) * 100;
                }
            };
            pivotGridControl.CustomDrawCell += (s, e) => {
                if (e.DataField == field ) {
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                }
            };
        }
    }
}