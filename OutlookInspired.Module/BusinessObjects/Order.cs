using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Services;


namespace OutlookInspired.Module.BusinessObjects{
    [XafDefaultProperty(nameof(InvoiceNumber))]
    [CloneView(CloneViewType.DetailView, OrderInvoiceDetailView)]
    [CloneView(CloneViewType.DetailView, OrderGridViewDetailView)]
    [CloneView(CloneViewType.DetailView, OrderDetailViewMaps)]
    [ImageName("BO_Order")]
    public class Order :OutlookInspiredBaseObject, IViewFilter,IRouteMapsMarker{
        public const string OrderDetailViewMaps = "Order_DetailView_Maps";
        public const string OrderInvoiceDetailView = "Order_DetailView_Child";
        public const string OrderGridViewDetailView = "OrderGridView_DetailView";
        
        [XafDisplayName("Invoice #")]
        [FontSizeDelta(4)]
        public  virtual string InvoiceNumber { get; set; }
        
        public virtual Customer Customer { get; set; }
        public virtual CustomerStore Store { get; set; }
        public  virtual string PONumber { get; set; }
        public virtual Employee Employee { get; set; }
        public  virtual DateTime OrderDate { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal SaleAmount { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal ShippingAmount { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal TotalAmount { get; set; }
        public  virtual DateTime? ShipDate { get; set; }
        public  virtual OrderShipMethod ShipMethod { get; set; }
        public  virtual string OrderTerms { get; set; }
        [Aggregated]
        public virtual ObservableCollection<OrderItem> OrderItems{ get; set; } = new();
        public  virtual ShipmentCourier ShipmentCourier { get; set; }
        public  virtual ShipmentStatus ShipmentStatus { get; set; }

        [VisibleInDetailView(false)]
        [XafDisplayName(nameof(ShipmentStatus))]
        public virtual byte[] ShipmentStatusImage => ShipmentStatus.ImageInfo().ImageBytes;
        public  virtual string Comments { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal RefundTotal { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal PaymentTotal { get; set; }
        [NotMapped]
        public PaymentStatus PaymentStatus 
            => PaymentTotal == decimal.Zero && RefundTotal == decimal.Zero ? PaymentStatus.Unpaid :
                RefundTotal == TotalAmount ? PaymentStatus.RefundInFull :
                PaymentTotal == TotalAmount ? PaymentStatus.PaidInFull : PaymentStatus.Other;

        [VisibleInDetailView(false)]
        [XafDisplayName(nameof(ShipmentStatus))]
        public byte[] PaymentStatusImage => PaymentStatus.ImageInfo().ImageBytes;
        [NotMapped]
        public double ActualWeight 
            => OrderItems == null ? 0 : OrderItems.Where(item => item.Product != null)
                    .Sum(item => item.Product.Weight * item.ProductUnits);

        string IBaseMapsMarker.Title => InvoiceNumber;

        double IBaseMapsMarker.Latitude => Store.Latitude;

        double IBaseMapsMarker.Longitude => Store.Longitude;
    }
    
    public enum OrderShipMethod {
        Ground, Air
    }
    public enum ShipmentCourier {
        None, FedEx, UPS, DHL
    }
    public enum ShipmentStatus {
        [ImageName("ShipmentAwaiting")]
        Awaiting,
        [ImageName("ShipmentTransit")]
        Transit,
        [ImageName("ShipmentReceived")]
        Received
    }
    public enum PaymentStatus {
        [ImageName("PaymentUnPaid")]
        Unpaid, 
        [ImageName("PaymentPaid")]
        PaidInFull, 
        [ImageName("PaymentRefund")]
        RefundInFull,
        
        Other
    }
}