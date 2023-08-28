using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace OutlookInspired.Module.BusinessObjects{
    public class OrderItem :OutlookInspiredBaseObject{
        public virtual Order Order { get; set; }

        public virtual Product Product{ get; set; }

        [Browsable(false)]
        public virtual Guid? ProductID { get; set; }
        
        public virtual  int ProductUnits { get; set; }
        [DataType(DataType.Currency)]
        public virtual  decimal ProductPrice { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal Discount { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal Total { get; set; }
    }
}