using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components.DevExtreme.Maps;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services.Internal;
using FeatureCollection = OutlookInspired.Blazor.Server.Services.FeatureCollection;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme.PivotGrid{
    public class Model:ComponentModelBase,IComponentContentHolder{
        
        
        public RenderFragment ComponentContent => this.Create(model => model.Create<DevExtremePivotGrid>());
    }
    
    public class PivotGridField {
        public string Caption { get; set; }
        public int? Width { get; set; }
        public string DataField { get; set; }
        public string Area { get; set; }
        public string DataType { get; set; }
        public string SummaryType { get; set; }
        public string Format { get; set; }
        // Add a delegate or an event for the 'selector' if needed
    }

    public class PivotGridDataSource {
        public List<PivotGridField> Fields { get; set; }
        public object Store { get; set; }  // Replace 'object' with the appropriate type for 'sales'
    }

    public class PivotGridOptions {
        public bool AllowSortingBySummary { get; set; }
        public bool AllowSorting { get; set; }
        public bool AllowFiltering { get; set; }
        public bool AllowExpandAll { get; set; }
        public int Height { get; set; }
        public bool ShowBorders { get; set; }
        public FieldChooserOptions FieldChooser { get; set; }
        public PivotGridDataSource DataSource { get; set; }
    }

    public class FieldChooserOptions {
        public bool Enabled { get; set; }
    }

}