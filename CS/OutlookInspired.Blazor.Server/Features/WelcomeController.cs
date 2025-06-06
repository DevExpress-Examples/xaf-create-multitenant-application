using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features {
    public class WelcomeController : ObjectViewController<DetailView, Welcome> {
        protected override void OnActivated() {
            base.OnActivated();
            View.CustomizeViewItemControl<ImagePropertyEditor>(this, item => {
                item.ComponentModel.CssClass = "welcome-image-viewer";
            });
        }
    }
}