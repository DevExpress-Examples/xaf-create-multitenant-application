using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using OutlookInspired.Blazor.Server.Editors;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Maps {
    [ListEditor(typeof(MapItem), true)]
    public class MapItemChartListEditor : ChartListEditor<MapItem, string, decimal, string> {
        public MapItemChartListEditor(IModelListView info) : base(info) { }
    }
}