using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Services{
    internal static class ActionExtensions{
        public static View NewDetailView(this ActionBaseEventArgs e,string viewId,TargetWindow targetWindow=TargetWindow.Default,bool isRoot=false){
            var detailView = e.ShowViewParameters.CreatedView = e.Application().NewDetailView(
                e.Action.View().SelectedObjects.Cast<object>().First(),
                (IModelDetailView)e.Application().FindModelView(viewId),isRoot);
            e.ShowViewParameters.TargetWindow = targetWindow;
            return detailView;
        }

        public static IEnumerable<ChoiceActionItem> ChoiceActionItem(this object[] objects) 
            => objects.Select(o => new ChoiceActionItem(o.ToString(),o));
        public static View View(this ActionBase actionBase) => actionBase.View<View>();
        public static XafApplication Application(this ActionBaseEventArgs actionBase) => actionBase.Action.Application;
        public static View View(this ActionBaseEventArgs actionBase) => actionBase.Action.View();
        public static T ParentObject<T>(this ActionBaseEventArgs actionBase) where T : class 
            => actionBase.Frame().ParentObject<T>();
        public static Frame Frame(this ActionBaseEventArgs actionBase) => actionBase.Action.Controller.Frame;

        public static T View<T>(this ActionBase actionBase) where T : View => actionBase.Controller.Frame?.View as T;
        



    }
}