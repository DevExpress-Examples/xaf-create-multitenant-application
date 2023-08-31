using DevExpress.ExpressApp.DC;

namespace OutlookInspired.Module.BusinessObjects{
    [DomainComponent]
    public class MapItem{
        public string City{ get; set; }
        public double Latitude{ get; set; }
        public double Longitude { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }

        public decimal Total { get; set; }

        public string CustomerName => Customer.Name;

        public string ProductName => Product.Name;

        public ProductCategory ProductCategory => Product.Category;
    }
}