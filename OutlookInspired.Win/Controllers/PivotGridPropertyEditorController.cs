using DevExpress.ExpressApp;
using DevExpress.ExpressApp.PivotGrid.Win;
using OutlookInspired.Win.Editors;
using OutlookInspired.Win.Extensions;

namespace OutlookInspired.Win.Controllers{
    public class PivotGridPropertyEditorController:ViewController<ListView>{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is PivotGridListEditor pivotGridListEditor){
                var pivotGridControl = pivotGridListEditor.PivotGridControl;
                var repositoryItems = pivotGridControl.AddRepositoryItems(View);
                pivotGridControl.CustomCellEdit += (_, e) => {
                    if (repositoryItems.TryGetValue(e.DataField, out var item)){
                        e.RepositoryItem = item;
                    }
                };
                pivotGridControl.CustomCellValue += (_, e) => {
                    if (repositoryItems.TryGetValue(e.DataField, out var item)&&item is IValueCalculator valueCalculator){
                        e.Value = valueCalculator.Calculate(e.Value);  
                    }
                };
                pivotGridControl.CustomDrawCell += (_, e) => {
                    if (repositoryItems.TryGetValue(e.DataField, out var item)){
                        e.Appearance = item.Appearance;
                    }
                };    
            }
        }
    }
}