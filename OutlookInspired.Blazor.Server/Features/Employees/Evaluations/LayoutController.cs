using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Employees.Evaluations{
    public class LayoutController:ObjectViewController<ListView,Evaluation>{
        public LayoutController() => TargetViewId = Evaluation.EmployeeEvaluationsChildListView;

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is not DxGridListEditor{ Control: IDxGridAdapter gridAdapter }) return;
            gridAdapter.GridSelectionColumnModel.Visible = false;
            var dataColumnModel = gridAdapter.GridDataColumnModels.First(model => model.FieldName == nameof(Evaluation.Subject));
            dataColumnModel.HeaderCaptionTemplate = _ => _ => { };
            dataColumnModel.CellDisplayTemplate = ColumnTemplate.Create;
        }
    }
}