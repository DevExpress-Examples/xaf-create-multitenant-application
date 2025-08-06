namespace OutlookInspired.Blazor.Server.Features.Quotes{
    public class MapOpportunitiesController:Module.Features.Quotes.MapOpportunitiesController{
        protected override void OnActivated(){
            base.OnActivated();
            Active["Blazor"] = false;
        }
    }
}