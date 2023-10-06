using System.Collections;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components;

namespace OutlookInspired.Blazor.Server.Editors {
    public abstract class ChartListEditor<TObject,TComponent> : ListEditor where TComponent : ComponentBase{
        protected ChartListEditor(IModelListView info) : base(info) { }
        public new ChartModel<TObject,TComponent> Control => (ChartModel<TObject, TComponent>)base.Control;
        protected override object CreateControlsCore() 
            => new ChartModel<TObject,TComponent>();

        protected override void AssignDataSourceToControl(Object dataSource){
            if (Control == null) return;
            Control.Data = dataSource as IEnumerable<TObject>;
        }

        public override void Refresh() { }
        public override object FocusedObject { get; set; }
        public override IList GetSelectedObjects() => Array.Empty<object>();
        public override SelectionType SelectionType => SelectionType.None;
    }


}