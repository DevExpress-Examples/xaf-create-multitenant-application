using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Services.Internal{
    internal static class XafApplicationExtensions{
        public static DetailView NewDetailView(this XafApplication application, Type objectType){
            var objectSpace = application.CreateObjectSpace(objectType);
            return application.CreateDetailView(objectSpace, objectSpace.CreateObject(objectType));
        }
        
        public static DetailView NewDetailView(this XafApplication application,object instance, IModelDetailView modelDetailView = null, bool isRoot = true) 
            => application.NewDetailView(space => space.GetObject(instance),modelDetailView,isRoot);

        public static DetailView NewDetailView<T>(this XafApplication application,Func<IObjectSpace,T> currentObjectFactory,IModelDetailView modelDetailView=null,bool isRoot=true){
            var objectSpace = application.CreateObjectSpace(modelDetailView?.ModelClass.TypeInfo.Type??typeof(T));
            var currentObject = currentObjectFactory(objectSpace);
            modelDetailView ??= application.FindModelDetailView(currentObject.GetType());
            var detailView = application.CreateDetailView(objectSpace, modelDetailView,isRoot);
            detailView.CurrentObject = objectSpace.GetObject(currentObject);
            return detailView;
        }
        
        public static void ShowViewInPopupWindow(this XafApplication application, object instance)
            => application.ShowViewStrategy.ShowViewInPopupWindow(application.NewDetailView(space => space.GetObject(instance)));
        
        public static IModelDetailView FindModelDetailView(this XafApplication application, Type objectType) 
            => (IModelDetailView) application.Model.Views[application.FindDetailViewId(objectType)];
        
        public static IObjectSpace NewObjectSpace(this XafApplication application) 
            => application.CreateObjectSpace(typeof(OutlookInspiredBaseObject));
    }
}