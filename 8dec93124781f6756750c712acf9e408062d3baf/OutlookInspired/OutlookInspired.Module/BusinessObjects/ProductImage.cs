

namespace OutlookInspired.Module.BusinessObjects{
    public class ProductImage :OutlookInspiredBaseObject{
        public virtual Picture Picture { get; set; }
        public virtual Product Product { get; set; }
        public virtual Guid? ProductId { get; set; }
        
    }
}