using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Templates;

namespace OutlookInspired.Blazor.Server.Controllers{
    public class PopupWindowSizeController:WindowController{
        public PopupWindowSizeController() => TargetWindowType=WindowType.Main;
        protected override void OnActivated(){
            base.OnActivated();
            Application.CustomizeTemplate+=ApplicationOnCustomizeTemplate;
        }

        protected override void OnDeactivated(){
            base.OnDeactivated();
            Application.CustomizeTemplate-=ApplicationOnCustomizeTemplate;
        }

        private void ApplicationOnCustomizeTemplate(object sender, CustomizeTemplateEventArgs e){
            if (e.Template is not IPopupWindowTemplateSize size) return;
            size.MaxWidth = "90vw";
            size.Width = "90vw";
            size.MaxHeight = "90vh";
            size.Height = "90vh";

        }
    }
}