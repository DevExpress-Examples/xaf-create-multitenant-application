using DevExpress.ExpressApp.SystemModule;

namespace OutlookInspired.Module.Services{
    public static class ControllerExtensions{
        public static void UseObjectDefaultDetailView(this NewObjectViewController controller){
            controller.ObjectCreating += (_, e) => e.Cancel = true;
            controller.NewObjectAction.Executed += (_, e) => {
                var objectSpace = controller.Application.NewObjectSpace();
                e.ShowViewParameters.CreatedView = controller.Application.CreateDetailView(objectSpace,objectSpace.CreateObject(controller.Frame.View.ObjectTypeInfo.Type));
            };
        }
    }
}