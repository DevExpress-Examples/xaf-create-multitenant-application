using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
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
		public virtual ICollection<CustomerEmployee> CustomerEmployees { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
		public virtual ICollection<Quote> Quotes { get; set; }
		public string CustomerName => Customer?.Name;

	}
}