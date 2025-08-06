namespace OutlookInspired.Blazor.Server.Features.Customers{
    public class MapCustomerController:Module.Features.Customers.MapCustomerController{
        protected override void OnActivated(){
            base.OnActivated();
            Active["Blazor"] = false;
        }
    }
}