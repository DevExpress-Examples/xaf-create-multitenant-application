using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Controllers{
    public class EmployeeCommunicationController:ObjectViewController<ObjectView,Employee>{
        public EmployeeCommunicationController(){
            NewAction(nameof(Employee.HomePhone),"icon-home-phone-16",_ => throw new UserFriendlyException($"Call {((Employee)View.CurrentObject).HomePhone}"));
            NewAction(nameof(Employee.MobilePhone),"icon-mobile-phone-16",_ => throw new UserFriendlyException($"Call {((Employee)View.CurrentObject).MobilePhone}"));
            NewAction(nameof(Employee.Skype),"Skype",_ => throw new UserFriendlyException($"Video Call {((Employee)View.CurrentObject).Skype}"));
            NewAction(nameof(Employee.Email),"icon-email-16",_ => throw new UserFriendlyException("Click the editor value"));
        }

        private void NewAction(string id,string image,Action<SimpleAction> executed){
            var simpleAction = new SimpleAction(this, id, id);
            simpleAction.ImageName = image;
            simpleAction.PaintStyle = ActionItemPaintStyle.Image;
            simpleAction.Executed += (_, _) => executed(simpleAction);
        }
    }
}