using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;

namespace OutlookInspired.Module.Services{
    public static class ControllerExtensions{
        public static void ChangeNewObjectDetailView(this NewObjectViewController newObjectViewController,string viewId){
            object newObject = null;
            newObjectViewController.ObjectCreating += (_, e) => e.ShowDetailView = false;
            newObjectViewController.ObjectCreated += (_, e) => newObject = e.CreatedObject;
            newObjectViewController.NewObjectAction.Executed += (_, e) => e.ShowViewParameters.CreatedView =
                newObjectViewController.Application.NewDetailView(newObject, (IModelDetailView)newObjectViewController.Application.FindModelView(viewId));
        }
    }
}