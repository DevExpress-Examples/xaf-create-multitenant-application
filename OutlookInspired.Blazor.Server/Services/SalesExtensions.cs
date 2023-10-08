namespace OutlookInspired.Blazor.Server.Services{

    public class FeatureCollection{
        public string Type{ get; set; } = "FeatureCollection";
        public List<Feature> Features{ get; set; }
        
    }

    public class Feature{
        public string Type{ get; set; } = "Feature";
        public Geometry Geometry{ get; set; }
        public Properties Properties{ get; set; }
    }

    public class Geometry{
        public string Type{ get; set; } = "Point";
        public List<double> Coordinates{ get; set; }
    }

    public class Properties{
        public string Tooltip{ get; set; }
        public List<decimal> Values{ get; set; }
        public string City{ get; set; }
    }
}