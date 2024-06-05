using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.SystemModule;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Controllers {
    public class WelcomePageNavigationItemController : WindowController {
        public WelcomePageNavigationItemController() {
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated() {
            base.OnActivated();

            Frame.GetController<ShowNavigationItemController>().CustomShowNavigationItem += WelcomePageNavigationItemController_CustomShowNavigationItem;
        }

        private void WelcomePageNavigationItemController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e) {
            if(Frame is BlazorWindow window && window.ActiveWindow?.View.CurrentObject is Welcome) {
                e.Handled = true;
            }
        }
    }
}
