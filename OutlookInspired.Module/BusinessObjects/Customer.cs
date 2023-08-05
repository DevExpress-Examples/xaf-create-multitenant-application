using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Attributes.Validation;
using OutlookInspired.Module.Services;


namespace OutlookInspired.Module.BusinessObjects {
	[ImageName("BO_Customer")]
	[CloneView(CloneViewType.DetailView, ChildDetailViewId)]
	[CloneView(CloneViewType.DetailView, CustomerLayoutViewDetailView)]
	[CloneView(CloneViewType.DetailView, CustomerGridViewDetailView)]
	[XafDefaultProperty(nameof(Name))]
	public class Customer:MigrationBaseObject{
		public const string ChildDetailViewId = "Customer_DetailView_Child";
		public const string CustomerGridViewDetailView = "CustomerGridView_DetailView";
		public const string CustomerLayoutViewDetailView = "CustomerLayoutView_DetailView";
		[FontSizeDelta(4)]
		public  virtual string HomeOfficeLine { get; set; }
		[XafDisplayName("City")]
		public  virtual string HomeOfficeCity { get; set; }
		[ZipCode]
		[XafDisplayName("ZipCode")]
		public  virtual string HomeOfficeZipCode { get; set; }
		[XafDisplayName("Address")]
		[Attributes.Validation.Phone][VisibleInListView(false)][VisibleInLookupListView(false)]
		
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
		[InverseProperty(nameof(Order.Customer))]
		[Aggregated]
		public virtual ObservableCollection<Order> Orders{ get; set; }

		[InverseProperty(nameof(Quote.Customer))]
		[Aggregated]
		public virtual ObservableCollection<Quote> Quotes{ get; set; } = new();

		[InverseProperty(nameof(CustomerStore.Customer))]
		[Aggregated]
		public virtual ObservableCollection<CustomerStore> CustomerStores{ get; set; } = new();
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		[FieldSize(-1)]
		public virtual string Profile { get; set; }
		[ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
			DetailViewImageEditorMode = ImageEditorMode.PictureEdit)]
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual byte[] Logo { get; set; }
		
		
	}
	public enum CustomerStatus {
		Active, Suspended
	}

}
