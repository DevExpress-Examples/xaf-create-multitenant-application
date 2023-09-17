using DevExpress.ExpressApp.DC;

namespace OutlookInspired.Module.BusinessObjects{

    [DomainComponent]
    public class MapItem{
        public string City{ get; set; }
        public double Latitude{ get; set; }
        public double Longitude { get; set; }
        public decimal Total { get; set; }
        public string CustomerName{ get; set; }
        public string ProductName{ get; set; }

        public ProductCategory ProductCategory{ get; set; }
    }
}