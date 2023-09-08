using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers.Maps;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers.Products{
    public class ReportController:ViewController{
        public ReportController(){
            TargetObjectType = typeof(Product);
            ReportAction = new SingleChoiceAction(this, "ProductReport", PredefinedCategory.Reports){
                ImageName = "BO_Report", SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects,PaintStyle = ActionItemPaintStyle.Image,
                Items ={
                    new ChoiceActionItem("Sales","Sales"){ImageName ="CustomerQuickSales"},
                    new ChoiceActionItem("Shippments","Orders"){ImageName = "ProductQuickShippments"},
                    new ChoiceActionItem("Comparisons","Profile"){ImageName = "ProductQuickComparisons"},
                    new ChoiceActionItem("Top Sales Person","Top Sales Person"){ImageName = "ProductQuicTopSalesperson"}
                },
                ItemType = SingleChoiceActionItemType.ItemIsOperation
            };
            ReportAction.Executed+=ReportActionOnExecuted;
        }

        public SingleChoiceAction ReportAction{ get; }

        private void ReportActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => ReportAction.ShowReportPreview((string)ReportAction.SelectedItem.Data=="Profile"?CriteriaOperator.FromLambda<Product>(
                product => product.ID == ((Product)View.CurrentObject).ID):CriteriaOperator.FromLambda<OrderItem>(
                orderItem => orderItem.Product.ID == ((Product)View.CurrentObject).ID));

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            Active[nameof(MapsViewController)] = Frame.GetController<MapsViewController>().MapItAction.Active;
        }
    }
}