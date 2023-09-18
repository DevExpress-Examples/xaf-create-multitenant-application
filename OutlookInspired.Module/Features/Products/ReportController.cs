using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services;
using static OutlookInspired.Module.Services.ReportsExtensions;

namespace OutlookInspired.Module.Features.Products{
    public class ReportController:ViewController{
        public const string ReportActionId = "ProductReport";
        public ReportController(){
            TargetObjectType = typeof(Product);
            ReportAction = new SingleChoiceAction(this, ReportActionId, PredefinedCategory.Reports){
                ImageName = "BO_Report", SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects,PaintStyle = ActionItemPaintStyle.Image,
                Items ={
                    new ChoiceActionItem("Sales",Sales){ImageName ="CustomerQuickSales"},
                    new ChoiceActionItem("Shippments",OrdersReport){ImageName = "ProductQuickShippments"},
                    new ChoiceActionItem("Comparisons",ProductProfile){ImageName = "ProductQuickComparisons"},
                    new ChoiceActionItem("Top Sales Person",TopSalesPerson){ImageName = "ProductQuicTopSalesperson"}
                },
                ItemType = SingleChoiceActionItemType.ItemIsOperation
            };
            ReportAction.Executed+=ReportActionOnExecuted;
        }

        public SingleChoiceAction ReportAction{ get; }

        private void ReportActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => ReportAction.ShowReportPreview((string)ReportAction.SelectedItem.Data==ProductProfile?CriteriaOperator.FromLambda<Product>(
                product => product.ID == ((Product)View.CurrentObject).ID):CriteriaOperator.FromLambda<OrderItem>(
                orderItem => orderItem.Product.ID == ((Product)View.CurrentObject).ID));

        protected override void OnViewControllersActivated(){
            base.OnViewControllersActivated();
            Active[nameof(MapsViewController)] = Frame.GetController<MapsViewController>().MapItAction.Active;
            if (!Active)return;
            ReportAction.DisableReportItems();
        }
    }
}