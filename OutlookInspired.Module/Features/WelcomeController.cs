using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Features{
    public class WelcomeController:ObjectViewController<DetailView,Welcome>{
        protected override void OnActivated(){
            base.OnActivated();
            View.CurrentObject = new Welcome();
        }
    }
}