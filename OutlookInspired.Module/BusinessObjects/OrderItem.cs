using System.ComponentModel.DataAnnotations;

namespace OutlookInspired.Module.BusinessObjects{
    public class OrderItem :BaseObject{
        public virtual Order Order { get; set; }
        public virtual  long? OrderId { get; set; }
        public virtual Product Product { get; set; }
        public virtual  long? ProductId { get; set; }
        public virtual  int ProductUnits { get; set; }
        [DataType(DataType.Currency)]
        public virtual  decimal ProductPrice { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal Discount { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal Total { get; set; }
    }
}