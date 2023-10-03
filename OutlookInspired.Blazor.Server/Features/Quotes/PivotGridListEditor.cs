using System.Collections;
using DevExpress.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Quotes {

    [ListEditor(typeof(Quote),false)]
    public class PivotGridListEditor : ListEditor {
        public PivotGridListEditor(IModelListView info) : base(info) { }
        public new XafPivotGridAdapter<Quote> Control => (XafPivotGridAdapter<Quote>)base.Control;
        protected override object CreateControlsCore() 
            => new XafPivotGridAdapter<Quote>(new XafPivotGridModel<Quote>{
                Fields = new[] {
                    new XafPivotGridFieldModel() { Name = nameof(Quote.Opportunity), Area = PivotGridFieldArea.Data },
                    new XafPivotGridFieldModel() { Name = $"{nameof(Quote.CustomerStore)}.{nameof(CustomerStore.City)}", Area = PivotGridFieldArea.Row,Caption = nameof(CustomerStore.City)},
                    new XafPivotGridFieldModel() { Name = $"{nameof(Quote.CustomerStore)}.{nameof(CustomerStore.State)}", Area = PivotGridFieldArea.Row,Caption = nameof(CustomerStore.State)},
                    new XafPivotGridFieldModel() { Name = nameof(Quote.Total), Area = PivotGridFieldArea.Data }
                }
            });

        protected override void AssignDataSourceToControl(Object dataSource){
            if (Control == null || dataSource is not QueryableCollection queryableCollection) return;
            Control.Model.Data = (IEnumerable<Quote>)queryableCollection.Queryable;
        }

        protected override void OnDataSourceChanged() {
            base.OnDataSourceChanged();
            Control?.Model.Update();
        }

        public override void Refresh() { }
        public override object FocusedObject { get; set; }
        public override IList GetSelectedObjects() => Array.Empty<object>();
        public override SelectionType SelectionType => SelectionType.None;
    }

    public class XafPivotGridAdapter<T> : IComponentContentHolder {
        public XafPivotGridAdapter(XafPivotGridModel<T> model) => Model = model;

        public XafPivotGridModel<T> Model{ get; }

        public RenderFragment ComponentContent => XafPivotGrid<T>.Create(Model);
    }

}