namespace OutlookInspired.Blazor.Server.Features.Products{
    public class MapProductController:Module.Features.Products.MapProductController{
        protected override void OnActivated(){
            base.OnActivated();
            Active["Blazor"] = false;
        }
    }
}