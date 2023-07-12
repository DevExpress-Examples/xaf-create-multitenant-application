using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace OutlookInspired.Module.BusinessObjects{
    public class Product :MyBaseObject{
        public  virtual string Name { get; set; }
        public  virtual string Description { get; set; }
        public  virtual DateTime ProductionStart { get; set; }
        public  virtual bool Available { get; set; }
        public  virtual byte[] Image { get; set; }
        public virtual Employee Support { get; set; }
        public  virtual long? SupportId { get; set; }
        public virtual Employee Engineer { get; set; }
        public  virtual long? EngineerId { get; set; }
        public  virtual int? CurrentInventory { get; set; }
        public  virtual int Backorder { get; set; }
        public  virtual int Manufacturing { get; set; }
        public  virtual byte[] Barcode { get; set; }
        public virtual Picture PrimaryImage { get; set; }
        public  virtual long? PrimaryImageId { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal Cost { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal SalePrice { get; set; }
        [DataType(DataType.Currency)]
        public  virtual decimal RetailPrice { get; set; }
        public  virtual double Weight { get; set; }
        public  virtual double ConsumerRating { get; set; }
        public  virtual ProductCategory Category { get; set; }
        [InverseProperty(nameof(ProductCatalog.Product))]
        public virtual List<ProductCatalog> Catalog { get; set; }
        [InverseProperty(nameof(OrderItem.Product))]
        public virtual List<OrderItem> OrderItems { get; set; }
        public virtual List<ProductImage> Images { get; set; }
        public virtual ICollection<QuoteItem> QuoteItems { get; set; }
        public Stream Brochure => Catalog is{ Count: > 0 } ? Catalog[0].PdfStream : null;
        Image _img;
        public Image ProductImage => _img != null || PrimaryImage == null ? _img : _img = CreateImage(PrimaryImage.Data);

        Image CreateImage(byte[] data){
            if(data == null)
                throw new NotImplementedException();
                // return ResourceImageHelper.CreateImageFromResourcesEx("DevExpress.DevAV.Resources.Unknown-user.png", typeof(Employee).Assembly);
            return DevExpress.XtraEditors.Controls.ByteImageConverter.FromByteArray(data);
        }
    }

    public enum ProductCategory {
        Automation, Monitors, Projectors, Televisions, VideoPlayers,
    }

}