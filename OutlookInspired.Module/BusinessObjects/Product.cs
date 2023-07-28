using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using DevExpress.ExpressApp.DC;
using OutlookInspired.Module.Services;


namespace OutlookInspired.Module.BusinessObjects{
    public class Product :MigrationBaseObject{
        public  virtual string Name { get; set; }
        public  virtual string Description { get; set; }
        public  virtual DateTime ProductionStart { get; set; }
        public  virtual bool Available { get; set; }
        public  virtual byte[] Image { get; set; }
        public virtual Employee Support { get; set; }
        public virtual Employee Engineer { get; set; }
        [Browsable(false)]
        public virtual Guid? EngineerId { get; set; }
        public  virtual int? CurrentInventory { get; set; }
        public  virtual int Backorder { get; set; }
        public  virtual int Manufacturing { get; set; }
        public  virtual byte[] Barcode { get; set; }
        public virtual Picture PrimaryImage { get; set; }
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
        public virtual ObservableCollection<ProductCatalog> Catalogs{ get; set; } = new();

        [InverseProperty(nameof(OrderItem.Product))][Aggregated]
        public virtual ObservableCollection<OrderItem> OrderItems{ get; set; } = new();

        [Aggregated]
        public virtual ObservableCollection<ProductImage> Images{ get; set; } = new();

        [Aggregated]
        public virtual ObservableCollection<QuoteItem> QuoteItems{ get; set; } = new();
        public Stream Brochure => Catalogs is{ Count: > 0 } ? Catalogs[0].PdfStream : null;
        Image _img;
        public Image ProductImage => _img != null || PrimaryImage == null ? _img : _img = PrimaryImage.Data.CreateImage();

    }

    public enum ProductCategory {
        Automation, Monitors, Projectors, Televisions, VideoPlayers,
    }

}