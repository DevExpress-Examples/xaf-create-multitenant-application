using DevExpress.ExpressApp.Blazor.Components.Models;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme{
    public class Location{
        public double Lat{ get; set; }
        public double Lng{ get; set; }
        
    }

    public class MapMarker{
        public Location Location{ get; init; }
    }

    public class MapRoute{
        public int Weight{ get; set; } = 6;
        public string Color{ get; set; } = "orange";
        public double Opacity{ get; set; } = 0.5;
        public string Mode{ get; set; }
        public List<Location> Locations{ get; set; } = new();
    }

    public class MapSettings{
        public Location Center{ get; set; }
        public string Provider{ get; set; } = "bing";
        public string ApiKey{ get; set; } = "AgPa0XVf4_HaN5BOPbTUw5KNvYEGOx-EftnjNRnCILfNgobxJC_deESiKqcfEgLd";
        public int Zoom{ get; set; } = 16;
        public bool Controls{ get; set; } = true;
        public List<MapMarker> Markers{ get; set; } = new();
        public List<MapRoute> Routes{ get; set; } = new();
    }
    public class Model:ComponentModelBase{
        public MapSettings MapSettings{
            get => GetPropertyValue<MapSettings>();
            set => SetPropertyValue(value);
        }

        public bool ChangeRouteMode{ get; set; }
        public bool PrintMap{
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }
    }
}