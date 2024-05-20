using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.SystemModule;

namespace OutlookInspired.Blazor.Server.Controllers {
    public class DxGridListEditorController : ViewController<ListView> {
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            if(View.Editor is DxGridListEditor editor) {
                editor.RowClickMode = RowClickMode.SelectOnSingleProcessOnDouble;
            }
        }
    }
}