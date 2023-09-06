using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers.Maps;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers.Orders{
    public class ReportController:ViewController{
        public ReportController(){
            TargetObjectType = typeof(Order);
            ReportAction = new SingleChoiceAction(this, "OrderReport", PredefinedCategory.Reports){
                ImageName = "BO_Report", SelectionDependencyType = SelectionDependencyType.Independent,PaintStyle = ActionItemPaintStyle.Image,
                Items ={
                    new ChoiceActionItem("Revenue",null){ImageName ="CostAnalysis", Items ={
                        new ChoiceActionItem("Report", "Revenue Report"){ImageName = "CustomerProfileReport"},
                        new ChoiceActionItem("Analysis", "Revenue Analysis"){ImageName = "SalesAnalysis"}
                    }},
                    new ChoiceActionItem("Report","Order"){ImageName = "CustomerProfileReport"}
                },
                ItemType = SingleChoiceActionItemType.ItemIsOperation
            };
            ReportAction.Executed+=ReportActionOnExecuted;
        }

        public SingleChoiceAction ReportAction{ get; }

        private void ReportActionOnExecuted(object sender, ActionBaseEventArgs e){
            var selectedItemData = (string)ReportAction.SelectedItem.Data;
            if (selectedItemData.Contains("Revenue")){
                var id = ((Order)View.CurrentObject).Customer.ID;
                ReportAction.ShowReportPreview(View.ObjectTypeInfo.Type,selectedItemData == "Revenue Analysis"
                    ? CriteriaOperator.FromLambda<OrderItem>(item => item.Order.Customer.ID == id)
                    : CriteriaOperator.Parse($"IsThisMonth([{nameof(OrderItem.Order)}.{nameof(Order.OrderDate)}])"));
            }
            else{
                e.NewDetailView(Order.InvoiceDetailView, TargetWindow.NewModalWindow);
            }
        }
        
        protected override void OnActivated(){
            base.OnActivated();
            Active[nameof(MapsViewController)] = Frame.GetController<MapsViewController>().MapItAction.Active;
        }
    }
}