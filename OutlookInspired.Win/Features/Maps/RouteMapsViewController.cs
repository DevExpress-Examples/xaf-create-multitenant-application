using System.Text.RegularExpressions;
using DevExpress.ExpressApp.Actions;
using DevExpress.Map.Native;
using DevExpress.Persistent.Base;
using DevExpress.XtraMap;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Win.Extensions.Internal;
using BingManeuverType = OutlookInspired.Module.BusinessObjects.BingManeuverType;

namespace OutlookInspired.Win.Features.Maps{
    public class RouteMapsViewController:WinMapsViewController<IRouteMapsMarker>,IMapsRouteController{
        private readonly BingGeocodeDataProvider _geocodeDataProvider=new(){BingKey = Module.Features.Maps.MapsViewController.Key};
        private readonly BingRouteDataProvider _routeDataProvider=new(){BingKey = Module.Features.Maps.MapsViewController.Key,RouteOptions = { DistanceUnit = DistanceMeasureUnit.Mile}};
        private readonly BingSearchDataProvider _searchDataProvider=new(){BingKey = Module.Features.Maps.MapsViewController.Key};
        
        private GeoPoint _currentObjectPoint;

        protected override void OnActivated(){
            base.OnActivated();
            if (!Active)return;
            _currentObjectPoint = ((IMapsMarker)View.CurrentObject).ToGeoPoint();
            MapsViewController.TravelModeAction.Executed+=TravelModeActionOnExecuted;
            _routeDataProvider.RouteCalculated+=OnRouteCalculated;
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

        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (!Active)return;
            _routeDataProvider.RouteCalculated-=OnRouteCalculated;
            MapsViewController.TravelModeAction.Executed-=TravelModeActionOnExecuted;
        }

        public event EventHandler<RouteCalculatedArgs> RouteCalculated;

        protected virtual void OnRouteCalculated(RouteCalculatedArgs e) => RouteCalculated?.Invoke(this, e);
    }
}