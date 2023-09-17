using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services;
using static OutlookInspired.Module.DatabaseUpdate.Updater;

namespace OutlookInspired.Module.Features.Orders{
    public class FollowUpController:ViewController{
        public FollowUpController(){
            TargetObjectType = typeof(Order);
            var refundAction = new SimpleAction(this, FollowUp, PredefinedCategory.Edit){
                ImageName = "ThankYouNote", SelectionDependencyType = SelectionDependencyType.RequireSingleObject,PaintStyle = ActionItemPaintStyle.Image,
            };
            refundAction.Executed+=EditInvoiceActionOnExecuted;
        }

        private void EditInvoiceActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => Frame.ShowInDocument("FollowUp");


        protected override void OnViewControllersActivated(){
            base.OnViewControllersActivated();
            Active[nameof(MapsViewController)] = Frame.GetController<MapsViewController>().MapItAction.Active;
        }
    }
}