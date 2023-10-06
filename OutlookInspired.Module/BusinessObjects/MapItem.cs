using DevExpress.ExpressApp.Data;
using DevExpress.ExpressApp.DC;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Module.BusinessObjects{

    [DomainComponent]
    public class MapItem{
        [Key]
        public int ID{ get; set; }
        public string City{ get; init; }
        public double Latitude{ get; init; }
        public double Longitude { get; init; }
        public decimal Total { get; init; }
        public string CustomerName{ get; init; }
        public string ProductName{ get; init; }

        public ProductCategory ProductCategory{ get; set; }
        public string Color{ get; set; }

        public string PropertyValue(Type type) 
            => type.MapItemProperty() == nameof(ProductName) ? ProductName : CustomerName;
    }
}