using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using OutlookInspired.Blazor.Server.Components.DevExtreme.Maps;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public abstract class RouteMapsViewController<T>:BlazorMapsViewController<T,DxMapModel,DxMap>,IMapsRouteController where T:IRouteMapsMarker{
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

        protected override DxMapModel CustomizeModel(DxMapModel model){
            CalculateRoute(model.Options = ((IMapsMarker)View.CurrentObject).DxMapOptions(
                ((IModelOptionsHomeOffice)Application.Model.Options).HomeOffice,
                (string)Frame.GetController<MapsViewController>().TravelModeAction.SelectedItem.Data));
            return model;
        }

        private void CalculateRoute(DxMapOptions options) 
            => this.Await(async () => OnRouteCalculated(await ObjectSpace.ManeuverInstructions(
                options.Markers.First().Location, options.Markers.Last().Location, options.Routes.First().Mode,
                options.ApiKey.Bing)));
        
        private void TravelModeActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => CustomizeModel().RouteMode = ((string)Frame.GetController<MapsViewController>().TravelModeAction.SelectedItem.Data).ToLower();

        public event EventHandler<RouteCalculatedArgs> RouteCalculated;

        protected virtual void OnRouteCalculated(RouteCalculatedArgs e) 
            => RouteCalculated?.Invoke(this, e);
    }
}