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
    public abstract class ChartListEditor<TObject,TComponent> : ListEditor {
        protected ChartListEditor(IModelListView info) : base(info) { }
        public new XafChartAdapter<TObject,TComponent> Control => (XafChartAdapter<TObject,TComponent>)base.Control;
        protected override object CreateControlsCore() 
            => new XafChartAdapter<TObject,TComponent>(new XafChartModel<TObject>());

        protected override void AssignDataSourceToControl(Object dataSource){
            if (Control == null) return;
            Control.Model.Data = dataSource as IEnumerable<TObject>;
        }

        public override void Refresh() { }
        public override object FocusedObject { get; set; }
        public override IList GetSelectedObjects() => Array.Empty<object>();
        public override SelectionType SelectionType => SelectionType.None;
    }

    public class XafChartAdapter<T,TComponent> : IComponentContentHolder {
        public XafChartAdapter(XafChartModel<T> model) => Model = model;

        public XafChartModel<T> Model{ get; }

        public RenderFragment ComponentContent => Model.Create(model => model.Create<XafChart<MapItem>>());
    }

}