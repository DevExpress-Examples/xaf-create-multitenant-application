using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace OutlookInspired.Module.Services{
    public static class ActionExtensions{
        public static View View(this ActionBase actionBase) => actionBase.View<View>();
        public static XafApplication Application(this ActionBaseEventArgs actionBase) => actionBase.Action.Application;
        public static View View(this ActionBaseEventArgs actionBase) => actionBase.Action.View();
        public static T ParentObject<T>(this ActionBaseEventArgs actionBase) where T : class 
            => actionBase.Frame().ParentObject<T>();
        public static Frame Frame(this ActionBaseEventArgs actionBase) => actionBase.Action.Controller.Frame;

        public static T View<T>(this ActionBase actionBase) where T : View => actionBase.Controller.Frame?.View as T;
        



    }
}