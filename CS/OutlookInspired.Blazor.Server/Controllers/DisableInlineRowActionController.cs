using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.SystemModule;

namespace OutlookInspired.Blazor.Server.Controllers {
    public class DisableInlineRowActionController : ViewController {
        protected override void OnActivated() {
            base.OnActivated();
            Frame.GetController<InlineRowActionController>()?.Active.SetItemValue("BlazorTemporary", false);
        }
    }
}