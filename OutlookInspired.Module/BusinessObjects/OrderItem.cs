using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.Attributes.Appearance;
using OutlookInspired.Module.Features.CloneView;


namespace OutlookInspired.Module.BusinessObjects{
    [VisibleInReports(true)][ImageName("BO_Sale")]
    [CloneView(CloneViewType.ListView,RecentOrderItemsListView )]
    [ForbidCRUD(true,RecentOrderItemsListView,"Product_OrderItems_ListView")]
    public class OrderItem :OutlookInspiredBaseObject{
        public const string RecentOrderItemsListView = "Recent_OrderItems_ListView";

        public virtual Order Order { get; set; }
        [Browsable(false)]
        public virtual Guid? OrderID { get; set; }
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