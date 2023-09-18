using System.Text.RegularExpressions;
using DevExpress.ExpressApp.Actions;
using DevExpress.Map.Native;
using DevExpress.Persistent.Base;
using DevExpress.XtraMap;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Win.Extensions;
using OutlookInspired.Win.Extensions.Internal;
using BingManeuverType = OutlookInspired.Module.BusinessObjects.BingManeuverType;

namespace OutlookInspired.Win.Controllers.Maps{
    public class RouteMapsViewController:WinMapsViewController<IRouteMapsMarker>,IMapsRouteController{
        private readonly BingGeocodeDataProvider _geocodeDataProvider=new(){BingKey = MapsViewController.Key};
        private readonly BingRouteDataProvider _routeDataProvider=new(){BingKey = MapsViewController.Key,RouteOptions = { DistanceUnit = DistanceMeasureUnit.Mile}};
        private readonly BingSearchDataProvider _searchDataProvider=new(){BingKey = MapsViewController.Key};
        
        private GeoPoint _currentObjectPoint;

        protected override void OnActivated(){
            base.OnActivated();
            if (!Active)return;
            _currentObjectPoint = ((IMapsMarker)View.CurrentObject).ToGeoPoint();
            MapsViewController.TravelModeAction.Executed+=TravelModeActionOnExecuted;
            _geocodeDataProvider.LocationInformationReceived+=OnLocationInformationReceived;
            _routeDataProvider.RouteCalculated+=OnRouteCalculated;
            _routeDataProvider.LayerItemsGenerating+=OnLayerItemsGenerating;
        }

        protected override void CustomizeMapControl(){
            MapControl.CenterPoint = ((IModelOptionsHomeOffice)Application.Model.Options).HomeOffice.ToGeoPoint();
            MapControl.Layers.AddRange(new LayerBase[]{
                new InformationLayer{ DataProvider = _geocodeDataProvider },
                new InformationLayer{ DataProvider = _searchDataProvider },
                RouteLayer()
            });
            CalculateRoute();
        }
        
        void CalculateRoute(){
            _routeDataProvider.RouteOptions.Mode = Enum.Parse<BingTravelMode>(MapsViewController
                .TravelModeAction.SelectedItem.Data.ToString()!);
            _routeDataProvider.CalculateRoute(new[]
                { new RouteWaypoint("Point A", (GeoPoint)MapControl.CenterPoint), new RouteWaypoint("Point B", _currentObjectPoint) }.ToList());
        }

        protected void TravelModeActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => CalculateRoute();

        private InformationLayer RouteLayer(){
            var routeLayer = new InformationLayer{
                DataProvider = _routeDataProvider, 
                HighlightedItemStyle ={ Stroke = Color.Cyan, StrokeWidth = 3 },
                ItemStyle = { Stroke = Color.Cyan,StrokeWidth = 3}
            };
            AddRoutePoints(routeLayer);
            return routeLayer;
        }

        public void AddRoutePoints(InformationLayer routeLayer){
            routeLayer.Data.Items.Clear();
            routeLayer.Data.Items.AddRange(new[]{
                new MapPushpin{ Text = "A", Location = MapControl.CenterPoint },
                new MapPushpin{ Text = "B", Location = _currentObjectPoint }
            });
        }
        
        private void OnLayerItemsGenerating(object sender, LayerItemsGeneratingEventArgs e){
            // e.Items.OfType<MapPushpin>().ForEach(pushpin => pushpin.Visible=false);
            // AddRoutePoints(_routeLayer);
        }

        private static readonly Regex RemoveTagRegex = new(@"<[^>]*>", RegexOptions.Compiled);
        private void OnRouteCalculated(object sender, BingRouteCalculatedEventArgs e){
            if(e.Error != null || e.Cancelled || e.CalculationResult is not{ ResultCode: RequestResultCode.Success })
                return;
            var bingRouteResult = e.CalculationResult.RouteResults.First();
            OnRouteCalculated(new RouteCalculatedArgs(bingRouteResult.Legs.SelectMany(leg => leg.Itinerary)
                .Select(item => {
                    var point = ObjectSpace.CreateObject<RoutePoint>();
                    point.ManeuverInstruction = RemoveTagRegex.Replace(item.ManeuverInstruction, string.Empty);
                    point.Distance = (item.Distance > 0.9) ? $"{Math.Ceiling(item.Distance):0} mi"
                        : $"{Math.Ceiling(item.Distance * 52.8) * 100:0} ft";
                    point.Maneuver = (BingManeuverType)item.Maneuver;
                    return point;
                }).ToArray(),bingRouteResult.Distance,bingRouteResult.Time,(TravelMode)_routeDataProvider.RouteOptions.Mode));
            Zoom.To((GeoPoint)MapControl.CenterPoint, _currentObjectPoint);
        }

        private void OnLocationInformationReceived(object sender, LocationInformationReceivedEventArgs e){
            if(e.Error != null || e.Cancelled || e.Result == null || e.Result.ResultCode != RequestResultCode.Success)
                return;
            var locations = e.Result.Locations;
            if(locations.Length > 0) {
                // LocationInformation loc = locations[0];
                throw new NotImplementedException();
                // ViewModel.PointB = new Address()
                // {
                //     Line = loc.Address.FormattedAddress,
                //     Latitude = loc.Location.Latitude,
                //     Longitude = loc.Location.Longitude,
                // };
            }

        }
        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (!Active)return;
            _geocodeDataProvider.LocationInformationReceived-=OnLocationInformationReceived;
            _routeDataProvider.RouteCalculated-=OnRouteCalculated;
            _routeDataProvider.LayerItemsGenerating-=OnLayerItemsGenerating;
            MapsViewController.TravelModeAction.Executed-=TravelModeActionOnExecuted;
        }

        public event EventHandler<RouteCalculatedArgs> RouteCalculated;

        protected virtual void OnRouteCalculated(RouteCalculatedArgs e) => RouteCalculated?.Invoke(this, e);
    }
}