using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;

namespace OutlookInspired.Blazor.Server.Features{
    public class MapsViewController:Module.Features.Maps.MapsViewController{
        protected override PredefinedCategory PopupActionsCategory() => PredefinedCategory.PopupActions;

        protected override string FrameContext() => TemplateContext.PopupWindowContextName;

        protected override void OnActivated(){
            base.OnActivated();
            PrintPreviewMapAction.Active[GetType().Name] = false;
            ExportMapAction.Active[GetType().Name] = false;
        }
    }
}