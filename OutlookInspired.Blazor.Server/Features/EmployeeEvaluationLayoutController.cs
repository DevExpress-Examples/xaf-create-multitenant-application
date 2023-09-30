using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using OutlookInspired.Blazor.Server.Components.Evaluations;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Controllers{
    public class EmployeeEvaluationLayoutController:ObjectViewController<ListView,Evaluation>{
        public EmployeeEvaluationLayoutController(){
            TargetViewNesting=Nesting.Nested;
        }

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is not DxGridListEditor{ Control: IDxGridAdapter gridAdapter }) return;
            gridAdapter.GridSelectionColumnModel.Visible = false;
            var dataColumnModel = gridAdapter.GridDataColumnModels.First(model => model.FieldName == nameof(Evaluation.Subject));
            dataColumnModel.HeaderCaptionTemplate = _ => _ => { };
            dataColumnModel.CellDisplayTemplate = EvalTemplate.Create;
        }
    }
}