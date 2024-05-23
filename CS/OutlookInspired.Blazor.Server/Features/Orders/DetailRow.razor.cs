using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Orders {
    public class DetailRowController : ViewController<ListView> {
        public DetailRowController() => TargetViewId = Order.ListViewDetail;

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            if(View.Editor is DxGridListEditor editor) {
                editor.GridModel.AutoCollapseDetailRow = true;
                editor.GridModel.DetailRowTemplate = DetailRow.Create;
            }
        }
    }
}