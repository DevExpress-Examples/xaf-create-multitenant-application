using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Employees.Evaluations{
    public class CellDisplayTemplateController:Controllers.CellDisplayTemplateController{
        public CellDisplayTemplateController() => TargetViewId = Evaluation.EmployeeEvaluationsChildListView;

        protected override RenderFragment<GridDataColumnCellDisplayTemplateContext> Fragment() => ColumnTemplate.Create;
    }
}