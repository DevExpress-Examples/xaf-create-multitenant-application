using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl.EF;

namespace OutlookInspired.Module.Services{
    internal static class ActionExtensions{
        public static void ShowReportPreview(this SingleChoiceAction action,Type reportDataType, CriteriaOperator criteria=null) 
            => action.Controller.Frame.GetController<ReportServiceController>()
                .ShowPreview(ReportDataProvider.GetReportStorage(action.Application.ServiceProvider)
                    .GetReportContainerHandle(action.View().ObjectSpace
                        .FindObject<ReportDataV2>(data =>data.DataTypeName==reportDataType.FullName&& data.DisplayName == (string)action.SelectedItem.Data)),criteria);

        public static View NewDetailView(this ActionBaseEventArgs e,string viewId,TargetWindow targetWindow=TargetWindow.Default,bool isRoot=false){
            e.ShowViewParameters.TargetWindow = targetWindow;
            return e.ShowViewParameters.CreatedView = e.Application().NewDetailView(
                e.Action.View().SelectedObjects.Cast<object>().SwitchIfEmpty(e.Action.View().Objects<object>()).First(),
                (IModelDetailView)e.Application().FindModelView(viewId),isRoot);
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