using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using DevExpress.Persistent.Validation;

namespace OutlookInspired.Module.BusinessObjects {
	[DefaultProperty(nameof(FullName))]
	public class CustomerEmployee :BaseObject{
		[RuleRequiredField]
		public virtual string FirstName { get; set; }
		[RuleRequiredField]
		public virtual string LastName { get; set; }
		public virtual string FullName { get; set; }
		public virtual PersonPrefix Prefix { get; set; }
		[RuleRequiredField, Attributes.Validation.Phone]
		public virtual string MobilePhone { get; set; }
		[RuleRequiredField, Attributes.Validation.EmailAddress]
		public virtual string Email { get; set; }
		public virtual Picture Picture { get; set; }
		public virtual long? PictureId { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual long? CustomerId { get; set; }
		public virtual CustomerStore CustomerStore { get; set; }
		public virtual long? CustomerStoreId { get; set; }
		public virtual string Position { get; set; }
		public virtual bool IsPurchaseAuthority { get; set; }
		public virtual ICollection<CustomerCommunication> CustomerCommunications { get; set; }
		public virtual Address Address => CustomerStore?.Address;
		public virtual ICollection<EmployeeTask> EmployeeTasks { get; set; }
		Image _photo;
		[NotMapped]
		public Image Photo {
			get => _photo ??= Picture.CreateImage();
			set {
				_photo = value;
				Picture = value.FromImage();
			} 
		}
	}
	public enum PersonPrefix {
		Dr, Mr, Ms, Miss, Mrs
	}

}
