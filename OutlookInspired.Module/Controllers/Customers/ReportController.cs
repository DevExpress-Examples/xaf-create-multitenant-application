using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers.Maps;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers.Customers{
    public class ReportController:ViewController{
        public const string ReportActionId = "CustomerReport";
        public ReportController(){
            TargetObjectType = typeof(Customer);
            ReportAction = new SingleChoiceAction(this, ReportActionId, PredefinedCategory.Reports){
                ImageName = "BO_Report", SelectionDependencyType = SelectionDependencyType.RequireSingleObject,PaintStyle = ActionItemPaintStyle.Image,
                Items ={
                    new ChoiceActionItem("Sales","Sales Summary Report"){ImageName ="CustomerQuickSales"},
                    new ChoiceActionItem("Employees","Contacts"){ImageName = "EmployeeProfile"},
                    new ChoiceActionItem("Locations","Locations"){ImageName = "CustomerQuickLocations"}
                },
                ItemType = SingleChoiceActionItemType.ItemIsOperation
            };
            ReportAction.Executed+=ReportActionOnExecuted;
        }

        public SingleChoiceAction ReportAction{ get; }

        private void ReportActionOnExecuted(object sender, ActionBaseEventArgs e){
            var selectedItemData = (string)ReportAction.SelectedItem.Data;
            if (selectedItemData == "Sales Summary Report"){
                ReportAction.ShowReportPreview(View.ObjectTypeInfo.Type, CriteriaOperator.FromLambda<OrderItem>(item 
                    => item.Order.Customer.ID == ((Customer)View.CurrentObject).ID));
            }
            else if (selectedItemData == "Locations"){
                ReportAction.ShowReportPreview(View.ObjectTypeInfo.Type,CriteriaOperator.FromLambda<Customer>(customer
                    => customer.ID == ((Customer)View.CurrentObject).ID));
            }
            else if (selectedItemData == "Contacts"){
                ReportAction.ShowReportPreview(View.ObjectTypeInfo.Type,CriteriaOperator.FromLambda<CustomerEmployee>(customerEmployee
                    => customerEmployee.Customer.ID == ((Customer)View.CurrentObject).ID));
            }
        }
        
        protected override void OnActivated(){
            base.OnActivated();
            Active[nameof(MapsViewController)] = Frame.GetController<MapsViewController>().MapItAction.Active;
        }
    }
}