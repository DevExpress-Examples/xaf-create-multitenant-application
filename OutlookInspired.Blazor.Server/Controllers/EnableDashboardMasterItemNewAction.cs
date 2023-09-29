using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Controllers{
    public class EnableDashboardMasterItemNewAction:ViewController<DashboardView>{
        protected override void OnActivated(){
            base.OnActivated();
            View.MasterItem().ControlCreated+=OnControlCreated;
        }

        protected override void OnDeactivated(){
            base.OnDeactivated();
            View.MasterItem().ControlCreated-=OnControlCreated;
        }

        private void OnControlCreated(object sender, EventArgs e) 
            => ((DashboardViewItem)sender).Frame.GetController<NewObjectViewController>().NewObjectAction.Active["ShowOnView"]=true;
    }
}