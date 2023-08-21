using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;

namespace OutlookInspired.Module.Controllers{
    public class EmployeeCommunicationController:ObjectViewController<ObjectView,BusinessObjects.Employee>{
        public EmployeeCommunicationController(){
            NewAction(nameof(BusinessObjects.Employee.HomePhone), "icon-home-phone-16",
                _ => throw new UserFriendlyException($"Call {((BusinessObjects.Employee)View.CurrentObject).HomePhone}"));
            NewAction(nameof(BusinessObjects.Employee.MobilePhone), "icon-mobile-phone-16",
                _ => throw new UserFriendlyException($"Call {((BusinessObjects.Employee)View.CurrentObject).MobilePhone}"));
            NewAction(nameof(BusinessObjects.Employee.Skype), "Skype",
                _ => throw new UserFriendlyException($"Video Call {((BusinessObjects.Employee)View.CurrentObject).Skype}"));
            NewAction(nameof(BusinessObjects.Employee.Email), "icon-email-16",
                _ => throw new UserFriendlyException("Click the editor value"));
        }

        private void NewAction(string id,string image,Action<SimpleAction> executed){
            var simpleAction = new SimpleAction(this, id, id);
            simpleAction.ImageName = image;
            simpleAction.PaintStyle = ActionItemPaintStyle.Image;
            simpleAction.Executed += (_, _) => executed(simpleAction);
        }
    }
}