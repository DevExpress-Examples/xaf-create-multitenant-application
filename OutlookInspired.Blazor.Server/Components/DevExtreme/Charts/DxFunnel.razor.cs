using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme.Charts{
    public class DxFunnelModel : ComponentModelBase, IComponentContentHolder{
        public FunnelChartOptions Options{ get; } = new();
        RenderFragment IComponentContentHolder.ComponentContent 
            => this.Create(model => model.Create<DxFunnel>());
    }

    public class FunnelChartOptions{
        public object[] DataSource { get; set; }
        public string Title{ get; set; }
        public Margin TitleMargin{ get; set; }
        public string ArgumentField{ get; set; }
        public string ValueField{ get; set; }
        public string[] Palette{ get; set; }
        public ExportOptions Export{ get; set; }=new();
        public TooltipOptions Tooltip{ get; set; }=new();
        public ItemOptions Item{ get; set; } = new();
        public LabelOptions Label{ get; set; } = new();
        public string Height{ get; set; }
    }

    public class Margin{
        public int Bottom{ get; set; }
    }

    public class ExportOptions{
        public bool Enabled{ get; set; }
    }

    public class TooltipOptions{
        public bool Enabled{ get; set; }
        public string Format{ get; set; }
    }

    public class ItemOptions{
        public BorderOptions Border{ get; set; } = new();
    }

    public class BorderOptions{
        public bool Visible{ get; set; }
    }

    public class LabelOptions{
        public bool Visible{ get; set; }
        public string Position{ get; set; }
        public string BackgroundColor{ get; set; }
    }
}