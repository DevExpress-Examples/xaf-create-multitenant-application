using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OutlookInspired.Module.BusinessObjects{
    public class Order :MigrationBaseObject{
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
        public virtual ObservableCollection<OrderItem> OrderItems{ get; set; } = new();
        public  virtual ShipmentCourier ShipmentCourier { get; set; }
        public  virtual ShipmentStatus ShipmentStatus { get; set; }
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

        [NotMapped]
        public double ActualWeight 
            => OrderItems == null ? 0 : OrderItems.Where(item => item.Product != null)
                    .Sum(item => item.Product.Weight * item.ProductUnits);
    }
    
    public enum OrderShipMethod {
        Ground, Air
    }
    public enum ShipmentCourier {
        None, FedEx, UPS, DHL
    }
    public enum ShipmentStatus {
        Awaiting, Transit, Received
    }
    public enum PaymentStatus {
        Unpaid, PaidInFull, RefundInFull, Other
    }
}