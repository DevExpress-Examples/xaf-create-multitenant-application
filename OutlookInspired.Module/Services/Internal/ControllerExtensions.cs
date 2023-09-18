using DevExpress.ExpressApp.SystemModule;

namespace OutlookInspired.Module.Services.Internal{
    internal static class ControllerExtensions{
        public static void UseObjectDefaultDetailView(this NewObjectViewController controller){
            void Handler(object sender, CreateCustomDetailViewEventArgs e){
                e.ViewId = controller.Application.FindDetailViewId(controller.Frame.View.ObjectTypeInfo.Type);
                controller.CreateCustomDetailView -= Handler;
            }
            controller.CreateCustomDetailView += Handler;
        }
    }
}