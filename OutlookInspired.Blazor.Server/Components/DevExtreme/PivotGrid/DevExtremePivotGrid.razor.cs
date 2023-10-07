using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme.PivotGrid{
    public class Model:ComponentModelBase,IComponentContentHolder{
        public PivotGridOptions Options{ get; } = new();
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(model => model.Create<DevExtremePivotGrid>());
    }
    
    public class PivotGridOptions {
        public bool AllowSortingBySummary { get; set; }
        public bool AllowSorting { get; set; }
        public bool AllowFiltering { get; set; }
        public bool AllowExpandAll { get; set; }
        public string Height{ get; set; } = "90vh";
        public bool ShowBorders { get; set; }
        public FieldChooserOptions FieldChooser { get;  } = new();
        public PivotGridDataSource DataSource { get;  } = new();
        public PivotGridScrolling Scrolling{ get; set; } = new();
    }

    
    public class PivotGridField {
        public string Caption { get; set; }
        public int? Width { get; set; }
        public bool IsProgressBar { get; set; }
        public string DataField { get; set; }
        public string Area { get; init; }
        public string DataType { get; set; }
        public string SummaryType { get; set; }
        public bool Expanded{ get; set; } = true;
        public object Format { get; set; }
        public string SortOrder{ get; set; }
    }


    public class PivotGridDataSource{
        public List<PivotGridField> Fields{ get; } = new();
        public List<PivotGridField> DataFields=>Fields.Where(field => field.Area=="data").ToList();
        public object Store{ get; set; }
    }
    
    public class PivotGridScrolling{
        public string Mode{ get; set; } = "standard";
    }

    public class FieldChooserOptions {
        public bool Enabled { get; set; }
    }

}