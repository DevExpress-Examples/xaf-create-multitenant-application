using DevExpress.DashboardExport.Map;
using MapItem = OutlookInspired.Module.BusinessObjects.MapItem;

namespace OutlookInspired.Blazor.Server.Services{
    class SalesExtensions{

        public void MethodName(MapItem[] mapItems){
            var featureCollection = new FeatureCollection
            {
                Type = "FeatureCollection",
                Features = mapItems.GroupBy(item => item.City)
                    .Select(cityItems => {
                        var mapItem = mapItems.First(mapItem => mapItem.City == cityItems.Key);
                        return new Feature{
                            Type = "Feature",
                            Geometry = new Geometry{
                                Type = "Point",
                                Coordinates = new List<double>{ mapItem.Latitude, mapItem.Longitude }
                            },
                            Properties = new Properties{
                                Values = cityItems.Select(item => item.Total).ToList()
                            }
                        };
                    }).ToList()
            };
        }

    }
    
    public class FeatureCollection
    {
        public string Type { get; set; }
        public List<Feature> Features { get; set; }
        public List<string> Names{ get; set; }
    }

    public class Feature
    {
        public string Type { get; set; }
        public Geometry Geometry { get; set; }
        public Properties Properties { get; set; }
    }

    public class Geometry
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }

    public class Properties
    {
        public string Tooltip { get; set; }
        public List<decimal> Values { get; set; }
        public string City{ get; set; }
    }
    
    
}