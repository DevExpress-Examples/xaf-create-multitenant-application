using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Services;


namespace OutlookInspired.Module.BusinessObjects{
    [ImageName("BO_Product")]
    [CloneView(CloneViewType.DetailView, ProductBrochureDetailView)]
    [CloneView(CloneViewType.DetailView, ProductCardViewDetailView)]
    [Appearance("UnAvailable",AppearanceItemType.ViewItem, "!"+nameof(Available),TargetItems = "*",FontStyle = FontStyle.Strikeout)]
    public class Product :MigrationBaseObject, IViewFilter{
        public const string ProductCardViewDetailView = "ProductCardView_DetailView";
        public const string ProductBrochureDetailView = "Product_Brochure_DetailView";
        public  virtual string Name { get; set; }
        [FieldSize(-1)]
        public  virtual string Description { get; set; }
        public  virtual DateTime ProductionStart { get; set; }
        public  virtual bool Available { get; set; }
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit,ImageSizeMode = ImageSizeMode.Zoom)]
        public  virtual byte[] Image { get; set; }
        public virtual Employee Support { get; set; }
        public virtual Employee Engineer { get; set; }
        [Browsable(false)]
        public virtual Guid? EngineerId { get; set; }
        [XafDisplayName("Inventory")]
        public  virtual int? CurrentInventory { get; set; }
        public  virtual int Backorder { get; set; }
        public  virtual int Manufacturing { get; set; }
        
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

        [InverseProperty(nameof(ProductCatalog.Product))][Aggregated]
        public virtual ObservableCollection<ProductCatalog> Catalogs{ get; set; } = new();

        [InverseProperty(nameof(OrderItem.Product))][Aggregated]
        public virtual ObservableCollection<OrderItem> OrderItems{ get; set; } = new();

        [Aggregated]
        public virtual ObservableCollection<ProductImage> Images{ get; set; } = new();

        [Aggregated]
        public virtual ObservableCollection<QuoteItem> QuoteItems{ get; set; } = new();
        [EditorAlias(EditorAliases.PdfViewerEditor)]
        public byte[] Brochure => Catalogs.Select(catalog => catalog.PDF).FirstOrDefault();
        

    }

    public enum ProductCategory {
        Automation, Monitors, Projectors, Televisions, VideoPlayers,
    }

}