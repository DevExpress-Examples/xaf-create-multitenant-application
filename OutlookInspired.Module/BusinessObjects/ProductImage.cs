namespace OutlookInspired.Module.BusinessObjects{
    public class ProductImage :BaseObject{
        public virtual Picture Picture { get; set; }
        public  virtual long? PictureId { get; set; }
        public virtual Product Product { get; set; }
        public  virtual long? ProductId { get; set; }
    }
}