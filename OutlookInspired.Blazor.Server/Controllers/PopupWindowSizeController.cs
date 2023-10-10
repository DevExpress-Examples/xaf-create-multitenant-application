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
            size.MaxWidth = "100vw";
            size.Width = "100vw";
            size.MaxHeight = "100vh";
            size.Height = "100vh";

        }
    }
}