using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using OutlookInspired.Blazor.Server.Components;
using OutlookInspired.Blazor.Server.Editors;
using OutlookInspired.Module.Features;

namespace OutlookInspired.Blazor.Server.Features{
    public class WelcomeController:Module.Features.WelcomeController{
        protected override void OnActivated(){
            base.OnActivated();
            View.CustomizeViewItemControl<PdfViewerEditor>(this, item => ((PdfModelAdapter)item.Control).Model.Style =
                    "position: absolute; top: 120px; left: 50%; transform: translate(-50%, 0); width: 500px; height: 305px;");
        }
    }
}