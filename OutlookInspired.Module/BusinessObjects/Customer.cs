using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Attributes.Validation;
using OutlookInspired.Module.Features.CloneView;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Features.ViewFilter;
using OutlookInspired.Module.Services.Internal;


namespace OutlookInspired.Module.BusinessObjects {
	[ImageName("BO_Customer")]
	[CloneView(CloneViewType.DetailView, ChildDetailView)]
	[CloneView(CloneViewType.DetailView, LayoutViewDetailView)]
	[CloneView(CloneViewType.DetailView, GridViewDetailView)]
	[CloneView(CloneViewType.DetailView, MapsDetailView)]
	[XafDefaultProperty(nameof(Name))]
	public class Customer:OutlookInspiredBaseObject,IViewFilter,ISalesMapsMarker{
		public const string ChildDetailView = "Customer_DetailView_Child";
		public const string GridViewDetailView = "CustomerGridView_DetailView";
		public const string LayoutViewDetailView = "CustomerLayoutView_DetailView";
		public const string MapsDetailView = "Customer_DetailView_Maps";
		[FontSizeDelta(4)]
		public  virtual string HomeOfficeLine { get; set; }
		[XafDisplayName("City")]
		public  virtual string HomeOfficeCity { get; set; }
		[ZipCode]
		[XafDisplayName("ZipCode")]
		public  virtual string HomeOfficeZipCode { get; set; }
		[XafDisplayName("Address")]
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public  virtual string BillingAddressLine { get; set; }
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual string BillingAddressCity { get; set; }
		[ZipCode][VisibleInListView(false)][VisibleInLookupListView(false)]
		public  virtual string BillingAddressZipCode { get; set; }
		[RuleRequiredField][XafDisplayName(nameof(Customer))]
		[FontSizeDelta(8)]
		public virtual string Name { get; set; }
		[XafDisplayName("State")]
		public virtual StateEnum HomeOfficeState { get; set; }
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual double HomeOfficeLatitude { get; set; }
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual double HomeOfficeLongitude { get; set; }
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual StateEnum BillingAddressState { get; set; }
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual double BillingAddressLatitude { get; set; }
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual double BillingAddressLongitude { get; set; }
		[NotMapped][VisibleInDetailView(false)]
		public virtual ObservableCollection<MapItem> CitySales{ get; set; } = new();
		[Aggregated]
		public virtual ObservableCollection<CustomerEmployee> Employees{ get; set; } = new(); 
		[Attributes.Validation.Phone]
		public virtual string Phone { get; set; }
		[Attributes.Validation.Phone][VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual string Fax { get; set; }
		[Attributes.Validation.Url]
		[EditorAlias(EditorAliases.HyperLinkPropertyEditor)][VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual string Website { get; set; }
		[DataType(DataType.Currency)][VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual decimal AnnualRevenue { get; set; }
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual int TotalStores { get; set; }
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual int TotalEmployees { get; set; }
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual CustomerStatus Status { get; set; }
		[InverseProperty(nameof(Quote.Customer))] [Aggregated]
		public virtual ObservableCollection<Quote> Quotes{ get; set; } = new();

		[InverseProperty(nameof(CustomerStore.Customer))]
		[Aggregated]
		public virtual ObservableCollection<CustomerStore> CustomerStores{ get; set; } = new();
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		[FieldSize(-1)]
		public virtual string Profile { get; set; }
		[ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
			DetailViewImageEditorMode = ImageEditorMode.PictureEdit,ImageSizeMode = ImageSizeMode.Zoom)]
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual byte[] Logo { get; set; }
		string IBaseMapsMarker.Title => Name;
		double IBaseMapsMarker.Latitude => BillingAddressLatitude;
		double IBaseMapsMarker.Longitude => BillingAddressLongitude;

		[InverseProperty(nameof(Order.Customer))]
		[Aggregated]
		public virtual ObservableCollection<Order> Orders{ get; set; } = new();
		[VisibleInDetailView(false)][NotMapped]
		public virtual List<Order> RecentOrders => ObjectSpace.GetObjectsQuery<Order>()
			.Where(order => order.Customer.ID == ID && order.OrderDate > DateTime.Now.AddMonths(-2)).ToList();

		Expression<Func<OrderItem, bool>> ISalesMapsMarker.SalesExpression => item => item.Order.Customer.ID == ID;
		
		IEnumerable<Order> ISalesMapsMarker.Orders => Orders;
	}
	public enum CustomerStatus {
		Active, Suspended
	}

}
