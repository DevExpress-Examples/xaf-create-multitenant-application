using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;


namespace OutlookInspired.Module.BusinessObjects {

	[DefaultClassOptions]
	public class Customer:MigrationBaseObject {
		public  virtual string HomeOfficeLine { get; set; }
		public  virtual string HomeOfficeCity { get; set; }
		public  virtual string HomeOfficeZipCode { get; set; }
		public  virtual string BillingAddressLine { get; set; }
		public virtual string BillingAddressCity { get; set; }
		public  virtual string BillingAddressZipCode { get; set; }
		[RuleRequiredField]
		public virtual string Name { get; set; }
		public virtual StateEnum HomeOfficeState { get; set; }
		public virtual double HomeOfficeLatitude { get; set; }
		public virtual double HomeOfficeLongitude { get; set; }
		public virtual StateEnum BillingAddressState { get; set; }
		public virtual double BillingAddressLatitude { get; set; }
		public virtual double BillingAddressLongitude { get; set; }
		[Aggregated]
		public virtual ObservableCollection<CustomerEmployee> Employees{ get; set; } 
		[Attributes.Validation.Phone]
		public virtual string Phone { get; set; }
		[Attributes.Validation.Phone]
		public virtual string Fax { get; set; }
		[Attributes.Validation.Url]
		public virtual string Website { get; set; }
		[DataType(DataType.Currency)]
		public virtual decimal AnnualRevenue { get; set; }
		public virtual int TotalStores { get; set; }
		public virtual int TotalEmployees { get; set; }
		public virtual CustomerStatus Status { get; set; }
		[InverseProperty(nameof(Order.Customer))]
		[Aggregated]
		public virtual ObservableCollection<Order> Orders{ get; set; } 
		[InverseProperty(nameof(Quote.Customer))]
		[Aggregated]
		public virtual ObservableCollection<Quote> Quotes { get; set; }
		[InverseProperty(nameof(CustomerStore.Customer))]
		[Aggregated]
		public virtual ObservableCollection<CustomerStore> CustomerStores { get; set; }
		public virtual string Profile { get; set; }
		[ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
			DetailViewImageEditorMode = ImageEditorMode.PictureEdit)]
		public virtual byte[] Logo { get; set; }
		
		
	}
	public enum CustomerStatus {
		Active, Suspended
	}

}
