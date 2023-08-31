using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.XtraMap;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Module.Services;
using OutlookInspired.Win.Extensions;

namespace OutlookInspired.Win.Controllers.Maps{
    public class RouteMapsViewController:WinMapsViewController<IRouteMapsMarker>{
        private readonly BingGeocodeDataProvider _geocodeDataProvider=new(){BingKey = MapsViewController.Key};
        private readonly BingRouteDataProvider _routeDataProvider=new(){BingKey = MapsViewController.Key,RouteOptions = { DistanceUnit = DistanceMeasureUnit.Mile}};
        private readonly BingSearchDataProvider _searchDataProvider=new(){BingKey = MapsViewController.Key};
        
        private GeoPoint _currentObjectPoint;

        
        protected override void OnActivated(){
            base.OnActivated();
            if (Frame is NestedFrame)return;
            var mapsViewController = Frame.GetController<MapsViewController>();
            Active[View.ObjectTypeInfo.Name] = mapsViewController.TravelModeAction.Active;
            _currentObjectPoint = ((IMapsMarker)View.CurrentObject).ToGeoPoint();
            mapsViewController.TravelModeAction.Executed+=TravelModeActionOnExecuted;
            _geocodeDataProvider.LocationInformationReceived+=OnLocationInformationReceived;
            _routeDataProvider.RouteCalculated+=OnRouteCalculated;
            _routeDataProvider.LayerItemsGenerating+=OnLayerItemsGenerating;
        }

        protected override void CustomizeMapControl(){
            MapControl.CenterPoint = ((IModelOptionsHomeOffice)Application.Model.Options).HomeOffice.ToGeoPoint();
            MapControl.Layers.AddRange(new LayerBase[]{
                new ImageLayer{ DataProvider = MapDataProvider },
                new InformationLayer{ DataProvider = _geocodeDataProvider },
                new InformationLayer{ DataProvider = _searchDataProvider },
                RouteLayer()
            });
            CalculateRoute();
        }
        
        void CalculateRoute(){
            _routeDataProvider.RouteOptions.Mode = Enum.Parse<BingTravelMode>(Frame.GetController<MapsViewController>()
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

        private void OnRouteCalculated(object sender, BingRouteCalculatedEventArgs e){
            if(e.Error != null || e.Cancelled || e.CalculationResult is not{ ResultCode: RequestResultCode.Success })
                return;
            // ViewModel.RouteDistance = routeResult.Distance;
            // ViewModel.RouteTime = routeResult.Time;
            // List<RoutePoint> routePoints = new List<RoutePoint>();
            // foreach(BingRouteLeg leg in routeResult.Legs)
            // foreach(BingItineraryItem item in leg.Itinerary)
            //     routePoints.Add(new RoutePoint(item));
            // UpdateRouteList(routePoints);
            ZoomToRegionService.ZoomTo((GeoPoint)MapControl.CenterPoint, _currentObjectPoint);
        }

        private void OnLocationInformationReceived(object sender, LocationInformationReceivedEventArgs e){
            if(e.Error != null || e.Cancelled || e.Result == null || e.Result.ResultCode != RequestResultCode.Success)
                return;
            LocationInformation[] locations = e.Result.Locations;
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
            if (Frame is NestedFrame)return;
            var mapsViewController = Frame.GetController<MapsViewController>();
            _geocodeDataProvider.LocationInformationReceived-=OnLocationInformationReceived;
            _routeDataProvider.RouteCalculated-=OnRouteCalculated;
            _routeDataProvider.LayerItemsGenerating-=OnLayerItemsGenerating;
            mapsViewController.TravelModeAction.Executed-=TravelModeActionOnExecuted;
        }
    }
}