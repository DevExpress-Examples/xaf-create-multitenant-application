using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Layout;
using DevExpress.Persistent.Base;
using OutlookInspired.Blazor.Server.Components.DevExtreme;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services.Internal;
using Model = OutlookInspired.Blazor.Server.Features.Maps.Model;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public class RouteMapsViewController:ObjectViewController<DetailView,IMapsMarker>,IMapsRouteController{
        protected override void OnDeactivated(){
            base.OnDeactivated();
            var mapsViewController = Frame.GetController<Module.Features.Maps.MapsViewController>();
            mapsViewController.TravelModeAction.Executed-=TravelModeActionOnExecuted;
            mapsViewController.PrintAction.Executed-=PrintActionOnExecuted;
        }

        protected override void OnActivated(){
            base.OnActivated();
            View.CustomizeViewItemControl<ControlViewItem>(this, item => {
                if (item.Control is not Model model) return;
                model.MapSettings=MapSettings();
                CalculateRoute(model);
            });
            var mapsViewController = Frame.GetController<Module.Features.Maps.MapsViewController>();
            mapsViewController.TravelModeAction.Executed+=TravelModeActionOnExecuted;
            mapsViewController.PrintAction.Executed+=PrintActionOnExecuted;
        }

        private void CalculateRoute(Model model) 
            => this.Await(async () => OnRouteCalculated(await ObjectSpace.ManeuverInstructions(
                model.MapSettings.Markers.First().Location, model.MapSettings.Markers.Last().Location, TravelMode,
                model.MapSettings.ApiKey)));

        private void PrintActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => ((Model)View.GetItems<ControlViewItem>().First().Control).PrintMap = true;

        private void TravelModeActionOnExecuted(object sender, ActionBaseEventArgs e){
            var control = (Model)View.GetItems<ControlViewItem>().First().Control;
            control.ChangeRouteMode = true;
            control.MapSettings = MapSettings();
            CalculateRoute(control);
        }

        private string TravelMode => (string)Frame.GetController<MapsViewController>().TravelModeAction.SelectedItem.Data;
        private MapSettings MapSettings() 
            => ((IMapsMarker)View.CurrentObject).MapSettings(((IModelOptionsHomeOffice)Application.Model.Options).HomeOffice, TravelMode);

        public event EventHandler<RouteCalculatedArgs> RouteCalculated;

        protected virtual void OnRouteCalculated(RouteCalculatedArgs e) 
            => RouteCalculated?.Invoke(this, e);
    }
}