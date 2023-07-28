

namespace OutlookInspired.Module.BusinessObjects{
    public class ProductImage :MigrationBaseObject{
        public virtual Picture Picture { get; set; }
        public virtual Product Product { get; set; }
        public virtual Guid? ProductId { get; set; }
        
    }
}