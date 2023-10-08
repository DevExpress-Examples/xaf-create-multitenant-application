namespace OutlookInspired.Blazor.Server.Components.DevExtreme.Maps{
    public class DxMap1Model : MapModel<DxMap1>{

        public DxMapOptions Options{ get; set; } = new();
    }

    public class DxMapOptions{
        public string Provider{ get; set; }
        public ApiKeyInfo ApiKey{ get; set; }
        public int Zoom{ get; set; }
        public int Height{ get; set; }
        public string Width{ get; set; }
        public bool Controls{ get; set; }
        public List<Marker> Markers{ get; set; }
        public List<Route> Routes{ get; set; }
    }

    public class ApiKeyInfo{
        public string Bing{ get; set; }
    }

    public class Marker{
        public string Location{ get; set; }
        // Additional properties like latitude and longitude can go here if you prefer to use different types for location.
    }

    public class Route{
        public int Weight{ get; set; }
        public string Color{ get; set; }
        public double Opacity{ get; set; }
        public string Mode{ get; set; }
        public List<object> Locations{ get; set; }
    }
}