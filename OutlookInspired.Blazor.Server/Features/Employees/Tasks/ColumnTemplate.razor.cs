using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Employees.Tasks{
    public class CellDisplayTemplateController:Controllers.CellDisplayTemplateController{
        public CellDisplayTemplateController() => TargetViewId = EmployeeTask.AssignedTasksChildListView;

        protected override RenderFragment<GridDataColumnCellDisplayTemplateContext> Fragment() => ColumnTemplate.Create;
    }

}