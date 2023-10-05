using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Editors {

    [ListEditor(typeof(MapItem),false)]
    public class ChartListEditor : ListEditor {
        public ChartListEditor(IModelListView info) : base(info) { }
        public new XafChartAdapter<MapItem> Control => (XafChartAdapter<MapItem>)base.Control;
        protected override object CreateControlsCore() 
            => new XafChartAdapter<MapItem>(new XafChartModel<MapItem>{
                 ArgumentField = item => item.CustomerName,
                 ValueField = item => item.Total,
                 NameField = item => item.CustomerName
            });

        protected override void AssignDataSourceToControl(Object dataSource){
            if (Control == null) return;
            Control.Model.Data = dataSource as IEnumerable<MapItem>;
        }


        public override void Refresh() { }
        public override object FocusedObject { get; set; }
        public override IList GetSelectedObjects() => Array.Empty<object>();
        public override SelectionType SelectionType => SelectionType.None;
    }

    public class XafChartAdapter<T> : IComponentContentHolder {
        public XafChartAdapter(XafChartModel<T> model) => Model = model;

        public XafChartModel<T> Model{ get; }

        public RenderFragment ComponentContent => Model.Create(model => model.Create<XafChart<MapItem>>());
    }

}