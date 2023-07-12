using System.ComponentModel.DataAnnotations;

namespace OutlookInspired.Module.BusinessObjects{
    public class Quote :MyBaseObject{
        public  virtual string Number { get; set; }
        public virtual Customer Customer { get; set; }
        public  virtual long? CustomerId { get; set; }
        public virtual CustomerStore CustomerStore { get; set; }
        public  virtual long? CustomerStoreId { get; set; }
        public virtual Employee Employee { get; set; }
        public  virtual long? EmployeeId { get; set; }
        public virtual DateTime Date { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal SubTotal { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal ShippingAmount { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal Total { get; set; }
        public virtual  double Opportunity { get; set; }
        public virtual List<QuoteItem> QuoteItems { get; set; }
    }
}