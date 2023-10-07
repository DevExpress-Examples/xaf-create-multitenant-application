using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.SystemModule;

namespace OutlookInspired.Win.Controllers{
    public class DisableSkinsController:WindowController{
        protected override void OnActivated(){
            base.OnActivated();
            Frame.GetController<ConfigureSkinController>().Active[nameof(DisableSkinsController)]=false;
        }
    }
}