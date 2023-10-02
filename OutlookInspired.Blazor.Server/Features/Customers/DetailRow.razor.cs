using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Customers{
    public class DetailRowController:ViewController<ListView>{
        public DetailRowController() => TargetViewId = $"{nameof(Customer)}_ListView";

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is not DxGridListEditor{ Control: IDxGridAdapter gridAdapter }) return;
            gridAdapter.GridModel.AutoCollapseDetailRow = true;
            gridAdapter.GridModel.DetailRowTemplate = DetailRow.Create;
        }
    }
}