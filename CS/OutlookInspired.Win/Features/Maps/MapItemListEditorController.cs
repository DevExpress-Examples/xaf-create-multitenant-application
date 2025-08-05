using DevExpress.ExpressApp;
using DevExpress.XtraMap;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Win.Editors.Maps;
using MapItem = OutlookInspired.Module.BusinessObjects.MapItem;

namespace OutlookInspired.Win.Features.Maps{
    public class MapItemListEditorController:ObjectViewController<ListView,MapItem>{
        private MapItemListEditor _mapItemListEditor;

        protected override void OnActivated(){
            base.OnActivated();
            _mapItemListEditor = View.Editor as MapItemListEditor;
            if (_mapItemListEditor == null){
                Active["editor"] = false;
                return;
            }
            _mapItemListEditor.CreateDataAdapter+=OnCreateAdapter;
        }

        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (_mapItemListEditor != null) _mapItemListEditor.CreateDataAdapter -= OnCreateAdapter;
        }

        private string GetPieSegmentPropertyName(){
            var isCustomer = ((PropertyCollectionSource)View.CollectionSource).MasterObject is Customer;
            return isCustomer ? nameof(MapItem.ProductName) : nameof(MapItem.CustomerName);
        }

        private void OnCreateAdapter(object sender, DataAdapterArgs e){
            _mapItemListEditor.ItemsLayer.ToolTipPattern = $"{nameof(MapItem.City)}:%A% {nameof(MapItem.Total)}:%V%";
            e.Adapter = new PieChartDataAdapter(){
                Mappings ={
                    Latitude = nameof(MapItem.Latitude), Longitude = nameof(MapItem.Longitude),
                    PieSegment = GetPieSegmentPropertyName(), Value = nameof(MapItem.Total)
                },
                PieItemDataMember = nameof(MapItem.City), SummaryFunction = SummaryFunction.Sum,
            };
        }

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            _mapItemListEditor.ItemsLayer.DataLoaded+=ItemsLayerOnDataLoaded;
        }

        private void ItemsLayerOnDataLoaded(object sender, DataLoadedEventArgs e){
            var items = ((ProxyCollection)_mapItemListEditor.DataSource).Cast<IMapItem>().ToArray();
            double[] bounds = MapItem.GetBounds(items);
            ZoomTo(new GeoPoint(bounds[1], bounds[0]), new GeoPoint(bounds[3], bounds[2]));
        }
        
        void ZoomTo(GeoPoint pointA, GeoPoint pointB, double margin = 0.2){
            if (pointA == null || pointB == null || _mapItemListEditor.ZoomService == null) return;
            var minLatitude = Math.Min(pointA.Latitude, pointB.Latitude);
            var maxLatitude = Math.Max(pointA.Latitude, pointB.Latitude);
            var latitudeDifference = maxLatitude - minLatitude;
            var lon1 = pointA.Longitude;
            var lon2 = pointB.Longitude;
            double longitudeSpan;
            double centerLongitude;
            bool crossesAntimeridian = Math.Abs(lon1 - lon2) > 180.0;
            if (crossesAntimeridian){
                longitudeSpan = 360.0 - Math.Abs(lon1 - lon2);
                var unwrappedCenter = (lon1 + lon2) / 2.0;
                centerLongitude = (Math.Abs(unwrappedCenter) < 90.0) ? unwrappedCenter + 180.0 : unwrappedCenter;
                if (centerLongitude > 180.0) centerLongitude -= 360.0;
                if (centerLongitude < -180.0) centerLongitude += 360.0;
            }
            else{
                longitudeSpan = Math.Abs(lon1 - lon2);
                centerLongitude = (lon1 + lon2) / 2.0;
            }
            var latitudePadding = CalculatePadding(margin, latitudeDifference);
            var longitudePadding = CalculatePadding(margin, longitudeSpan);
            var totalLatitudeSpan = latitudeDifference + 2 * latitudePadding;
            var totalLongitudeSpan = longitudeSpan + 2 * longitudePadding;
            
            const double minimumVisibleSpan = 0.01; 
            if (totalLatitudeSpan < minimumVisibleSpan){
                totalLatitudeSpan = minimumVisibleSpan;
            }
            if (totalLongitudeSpan < minimumVisibleSpan){
                totalLongitudeSpan = minimumVisibleSpan;
            }
            var centerLatitude = (minLatitude + maxLatitude) / 2.0;
            var southWestCorner = new GeoPoint(centerLatitude - totalLatitudeSpan / 2.0, centerLongitude - totalLongitudeSpan / 2.0);
            var northEastCorner = new GeoPoint(centerLatitude + totalLatitudeSpan / 2.0, centerLongitude + totalLongitudeSpan / 2.0);
            var centerPoint = new GeoPoint(centerLatitude, centerLongitude);
            
            _mapItemListEditor.ZoomService.ZoomToRegion(southWestCorner, northEastCorner, centerPoint);
        }
        static double CalculatePadding(double margin,double delta) 
            => delta > 0 ? Math.Max(0.1, delta * margin) : delta < 0 ? Math.Min(-0.1, delta * margin) : 0;

    }
}