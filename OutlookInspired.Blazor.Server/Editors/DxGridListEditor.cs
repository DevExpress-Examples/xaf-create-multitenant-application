using DevExpress.ExpressApp.Blazor.Editors.ActionControls;
using DevExpress.ExpressApp.Blazor.Editors.Models;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace OutlookInspired.Blazor.Server.Editors{
    [ListEditor(typeof(object),true)]
    public class DxGridListEditor:DevExpress.ExpressApp.Blazor.Editors.DxGridListEditor{
        public DxGridListEditor(IModelListView model) : base(model){
        }

        protected override object CreateControlsCore() => new DxGridAdapter(new DxGridModel());
    }

    public class DxGridAdapter:DevExpress.ExpressApp.Blazor.Editors.DxGridAdapter,ISupportInlineActions{
        public DxGridAdapter(DxGridModel gridModel) : base(gridModel){
        }

        void ISupportInlineActions.SetupActionColumn(ListEditorInlineActionControlContainer actionContainer){
            
        }
    }
}