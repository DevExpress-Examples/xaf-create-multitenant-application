using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DevExpress.Persistent.Base;


namespace OutlookInspired.Module.BusinessObjects{
    [VisibleInReports(true)][ImageName("BO_Sale")]
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