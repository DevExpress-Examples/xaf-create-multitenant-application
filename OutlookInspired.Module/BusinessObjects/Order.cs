using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using Newtonsoft.Json;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Features.CloneView;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Features.ViewFilter;
using OutlookInspired.Module.Services;
using OutlookInspired.Module.Services.Internal;
using EditorAliases = OutlookInspired.Module.Services.Internal.EditorAliases;


namespace OutlookInspired.Module.BusinessObjects{
    [XafDefaultProperty(nameof(InvoiceNumber))]
    [CloneView(CloneViewType.DetailView, ChildDetailView)]
    [CloneView(CloneViewType.DetailView, GridViewDetailView)]
    [CloneView(CloneViewType.DetailView, MapsDetailView)]
    [CloneView(CloneViewType.DetailView, InvoiceDetailView)]
    [ImageName("BO_Order")][VisibleInReports(true)]
    public class Order :OutlookInspiredBaseObject, IViewFilter,IRouteMapsMarker{
        public const string MapsDetailView = "Order_DetailView_Maps";
        public const string InvoiceDetailView = "Order_Invoice_DetailView";
        public const string ChildDetailView = "Order_DetailView_Child";
        public const string GridViewDetailView = "OrderGridView_DetailView";
        
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

        [EditorAlias(EditorAliases.PdfViewerEditor)]
        [VisibleInDetailView(false)]
        [NotMapped]
        public virtual byte[] ShipmentDetail{ get; set; } = Array.Empty<byte>();
        
        
        [EditorAlias(EditorAliases.PdfViewerEditor)]
        [VisibleInDetailView(false)]
        [NotMapped]
        public virtual byte[] InvoiceDocument{ get; set; } = Array.Empty<byte>();
        public  virtual string Comments { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal RefundTotal { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal PaymentTotal { get; set; }
        
        public PaymentStatus PaymentStatus 
            => PaymentTotal == decimal.Zero && RefundTotal == decimal.Zero ? PaymentStatus.Unpaid :
                RefundTotal == TotalAmount ? PaymentStatus.RefundInFull :
                PaymentTotal == TotalAmount ? PaymentStatus.PaidInFull : PaymentStatus.Other;

        [VisibleInDetailView(false)]
        [XafDisplayName(nameof(ShipmentStatus))]
        public byte[] PaymentStatusImage => PaymentStatus.ImageInfo().ImageBytes;
        
        public double ActualWeight 
            => OrderItems == null ? 0 : OrderItems.Where(item => item.Product != null)
                    .Sum(item => item.Product.Weight * item.ProductUnits);

        string IBaseMapsMarker.Title => InvoiceNumber;

        double IBaseMapsMarker.Latitude => Store?.Latitude??0;

        double IBaseMapsMarker.Longitude => Store?.Longitude??0;
    }
    
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum OrderShipMethod {
        Ground, Air
    }
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum ShipmentCourier {
        None, FedEx, UPS, DHL
    }
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum ShipmentStatus {
        [ImageName("ShipmentAwaiting")]
        Awaiting,
        [ImageName("ShipmentTransit")]
        Transit,
        [ImageName("ShipmentReceived")]
        Received
    }
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
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