

using System.ComponentModel;

namespace OutlookInspired.Module.BusinessObjects{
    public class ProductCatalog :OutlookInspiredBaseObject{
        public virtual Product Product { get; set; }
        [Browsable(false)]
        public virtual Guid? ProductId { get; set; }
        public  virtual byte[] PDF { get; set; }
        
    }
}