using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public class MapsViewController:Module.Features.Maps.MapsViewController{
        protected override PredefinedCategory PopupActionsCategory() => PredefinedCategory.PopupActions;

        protected override void Configure(ShowViewParameters parameters){
            base.Configure(parameters);
            var dialogController = Application.CreateController<DialogController>();
            // dialogController.CancelAction.Active[nameof(MapsViewController)] = false;
            dialogController.AcceptAction.Caption = "OK";
            parameters.Controllers.Add(dialogController);
        }

        protected override string FrameContext() => TemplateContext.PopupWindowContextName;

        protected override void OnActivated(){
            base.OnActivated();
            PrintPreviewMapAction.Active[GetType().Name] = false;
            ExportMapAction.Active[GetType().Name] = false;
        }
    }
}