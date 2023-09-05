using DevExpress.ExpressApp;
using OutlookInspired.Module.Resources.Reports;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers.Orders{
    public class ShipmentDetailController:ObjectViewController<DetailView,BusinessObjects.Order>{
        public ShipmentDetailController() => TargetViewId = BusinessObjects.Order.MapsDetailView;
        protected override void OnActivated(){
            base.OnActivated();
            using var fedExGroundLabel = new FedExGroundLabel();
            var order = ((BusinessObjects.Order)View.CurrentObject);
            fedExGroundLabel.DataSource = order.YieldItem();
            order.ShipmentDetail = fedExGroundLabel.ToPdf(order.WatermarkText());
        }
    }
}