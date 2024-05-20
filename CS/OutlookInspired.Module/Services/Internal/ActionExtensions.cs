using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl.EF;

namespace OutlookInspired.Module.Services.Internal{
    internal static class ActionExtensions{
        public static void TryExecute(this SimpleAction action){
            if (!action.Available()) return;
            action.DoExecute();
        }

        public static void DoExecute(this SingleChoiceAction action) 
            => action.DoExecute(action.SelectedItem);

        public static void DoExecute(this SingleChoiceAction action,Func<ChoiceActionItem,bool> selectItem){
            action.SelectedItem = action.Items.First(selectItem);
            action.DoExecute();
        }

        public static bool Available(this ActionBase action) => action.Active && action.Enabled;

        public static void ShowReportPreview(this SingleChoiceAction action,Type reportDataType, CriteriaOperator criteria=null,string parameterName=null)
            => action.HandleReportParameter(parameterName).Controller.Frame.GetController<ReportServiceController>()
                .ShowPreview(ReportDataProvider.GetReportStorage(action.Application.ServiceProvider)
                .GetReportContainerHandle(action.View().ObjectSpace.FindObject<ReportDataV2>(data
                        => data.DataTypeName == reportDataType.FullName &&
                           data.DisplayName == (string)action.SelectedItem.Data)), criteria);

        private static SingleChoiceAction HandleReportParameter(this SingleChoiceAction action, string parameterName){
            if (parameterName != null){
                var reportsDataSourceHelper = action.Application.Modules.FindModule<ReportsModuleV2>().ReportsDataSourceHelper;
                EventHandler<BeforeShowPreviewEventArgs> handler = null;
                handler = (_, e) => {
                    reportsDataSourceHelper.BeforeShowPreview -= handler;
                    e.Report.RequestParameters = false;
                    var reportParameter = e.Report.Parameters[parameterName];
                    if (reportParameter==null)return;
                    reportParameter.Visible = false;
                    reportParameter.Value = action.View().ObjectSpace
                        .GetKeyValue(action.View().SelectedObjects.Cast<object>().First());
                };
                reportsDataSourceHelper.BeforeShowPreview += handler;    
            }
            return action;
        }

        public static void ShowReportPreview(this SingleChoiceAction action,CriteriaOperator criteria=null,string parameterName=null) 
            => action.ShowReportPreview(action.View().ObjectTypeInfo.Type,criteria,parameterName);

        public static View NewDetailView(this ActionBaseEventArgs e,string viewId,TargetWindow targetWindow=TargetWindow.Default,bool isRoot=false){
            e.ShowViewParameters.TargetWindow = targetWindow;
            return e.ShowViewParameters.CreatedView = e.Application().NewDetailView(
                e.Action.View().SelectedObjects.Cast<object>().SwitchIfEmpty(e.Action.View().Objects<object>()).First(),
                (IModelDetailView)e.Application().FindModelView(viewId),isRoot);
        }
        
        public static View View(this ActionBase actionBase) => actionBase.View<View>();
        public static XafApplication Application(this ActionBaseEventArgs actionBase) => actionBase.Action.Application;
        public static View View(this ActionBaseEventArgs actionBase) => actionBase.Action.View();
        public static T ParentObject<T>(this ActionBaseEventArgs actionBase) where T : class 
            => actionBase.Frame().ParentObject<T>();
        public static Frame Frame(this ActionBaseEventArgs actionBase) => actionBase.Action.Controller.Frame;

        public static T View<T>(this ActionBase actionBase) where T : View => actionBase.Controller.Frame?.View as T;
        



    }
}