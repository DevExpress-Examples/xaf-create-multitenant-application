using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using OutlookInspired.Blazor.Server.Components.DevExtreme;
using OutlookInspired.Blazor.Server.Components.DevExtreme.Maps;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public class RouteMapsViewController:BlazorMapsViewController<IRouteMapsMarker>,IMapsRouteController{
        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (!Active)return;
            MapsViewController.TravelModeAction.Executed-=TravelModeActionOnExecuted;
        }

        protected override void OnActivated(){
            base.OnActivated();
            if(!Active)return;
            MapsViewController.TravelModeAction.Executed+=TravelModeActionOnExecuted;
        }

        protected override Model CustomizeModel(Model model){
            CalculateRoute(model.MapSettings = model.MapSettings = ((IMapsMarker)View.CurrentObject).MapSettings(
                    ((IModelOptionsHomeOffice)Application.Model.Options).HomeOffice, (string)Frame.GetController<MapsViewController>().TravelModeAction.SelectedItem.Data));
            return model;
        }

        private void CalculateRoute(MapSettings settings) 
            => this.Await(async () => OnRouteCalculated(await ObjectSpace.ManeuverInstructions(
                settings.Markers.First().Location, settings.Markers.Last().Location, settings.Routes.First().Mode,
                settings.ApiKey)));
        
        private void TravelModeActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => CustomizeModel().ChangeRouteMode = true;

        public event EventHandler<RouteCalculatedArgs> RouteCalculated;

        protected virtual void OnRouteCalculated(RouteCalculatedArgs e) 
            => RouteCalculated?.Invoke(this, e);
    }
}