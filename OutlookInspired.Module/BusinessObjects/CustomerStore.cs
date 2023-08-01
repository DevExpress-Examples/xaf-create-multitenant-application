using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using DevExpress.ExpressApp.DC;
using OutlookInspired.Module.Attributes.Validation;


namespace OutlookInspired.Module.BusinessObjects{
	public class CustomerStore :MigrationBaseObject{
		public virtual string AddressLine { get; set; }
		public virtual string AddressCity { get; set; }
		public virtual StateEnum AddressState { get; set; }
		[ZipCode]
		public virtual string ZipCode { get; set; }
		public virtual double AddressLatitude { get; set; }
		public virtual double AddressLongitude { get; set; }
		public virtual Customer Customer { get; set; }
		[Attributes.Validation.Phone]
		public virtual string Phone { get; set; }
		[Attributes.Validation.Phone]
		public virtual string Fax { get; set; }
		public virtual int TotalEmployees { get; set; }
		public virtual int SquereFootage { get; set; }
		[DataType(DataType.Currency)]
		public virtual decimal AnnualSales { get; set; }
		public virtual Crest Crest { get; set; }
		public virtual string Location { get; set; }
		[Aggregated]
		public virtual ObservableCollection<CustomerEmployee> CustomerEmployees{ get; set; } = new();
		[Aggregated]
		public virtual ObservableCollection<Order> Orders{ get; set; } = new();
		[Aggregated]
		public virtual ObservableCollection<Quote> Quotes{ get; set; } = new();
		

	}
}