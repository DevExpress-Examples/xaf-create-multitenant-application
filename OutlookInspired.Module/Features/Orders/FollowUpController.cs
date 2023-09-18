using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services;
using static OutlookInspired.Module.Services.MailMergeExtensions;

namespace OutlookInspired.Module.Features.Orders{
    public class FollowUpController:ViewController{
        private readonly SimpleAction _refundAction;

        public FollowUpController(){
            TargetObjectType = typeof(Order);
            _refundAction = new SimpleAction(this, FollowUp, PredefinedCategory.Edit){
                ImageName = "ThankYouNote", SelectionDependencyType = SelectionDependencyType.RequireSingleObject,PaintStyle = ActionItemPaintStyle.Image,
            };
            _refundAction.Executed+=EditInvoiceActionOnExecuted;
        }

        private void EditInvoiceActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => Frame.ShowInDocument(FollowUp);
        
        protected override void OnViewControllersActivated(){
            base.OnViewControllersActivated();
            Active[nameof(MapsViewController)] = Frame.GetController<MapsViewController>().MapItAction.Active;
            _refundAction.Enabled[nameof(FollowUpController)] = ObjectSpace.Any<RichTextMailMergeData>(data => data.Name == FollowUp);
        }
    }
}