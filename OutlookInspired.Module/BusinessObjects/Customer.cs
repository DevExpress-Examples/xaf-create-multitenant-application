using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.XtraEditors.Controls;

namespace OutlookInspired.Module.BusinessObjects {
	[DefaultClassOptions]
	public class Customer:BaseObject {
		public virtual int Id { get; set; }
		[RuleRequiredField]
		public virtual string Name { get; set; }
		public virtual Address HomeOffice{ get; set; } = new();
		public virtual Address BillingAddress{ get; set; } = new();
		public virtual StateEnum HomeOfficeState { get; set; }
		public virtual double HomeOfficeLatitude { get; set; }
		public virtual double HomeOfficeLongitude { get; set; }
		public virtual StateEnum BillingAddressState { get; set; }
		public virtual double BillingAddressLatitude { get; set; }
		public virtual double BillingAddressLongitude { get; set; }
		public virtual List<CustomerEmployee> Employees{ get; set; } = new();
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
		public virtual List<Order> Orders{ get; set; } = new();
		[InverseProperty(nameof(Quote.Customer))]
		public virtual List<Quote> Quotes { get; set; }
		[InverseProperty(nameof(CustomerStore.Customer))]
		public virtual List<CustomerStore> CustomerStores { get; set; }
		public virtual string Profile { get; set; }
		public virtual byte[] Logo { get; set; }
		Image _img;
		public Image Image => _img ??= CreateImage(Logo);
		static Image CreateImage(byte[] data){
			if(data == null)
				throw new NotImplementedException();
				// return ResourceImageHelper.CreateImageFromResourcesEx("DevExpress.DevAV.Resources.Unknown-user.png", typeof(Employee).Assembly);
			return ByteImageConverter.FromByteArray(data);
		}
	}
	public enum CustomerStatus {
		Active, Suspended
	}

}
