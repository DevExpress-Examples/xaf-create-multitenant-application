using DevExpress.ExpressApp;
using OutlookInspired.Blazor.Server.Components;
using OutlookInspired.Blazor.Server.Editors;

namespace OutlookInspired.Blazor.Server.Features{
    public class WelcomeController:Module.Features.WelcomeController{
        protected override void OnActivated(){
            base.OnActivated();
            View.CustomizeViewItemControl<PdfViewerEditor>(this, item => ((PdfModel)item.Control).Style =
                    "position: absolute; top: 120px; left: 50%; transform: translate(-50%, 0); width: 500px; height: 305px;");
        }
    }
}