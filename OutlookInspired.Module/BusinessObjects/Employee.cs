using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using DevExpress.Persistent.Validation;

namespace OutlookInspired.Module.BusinessObjects{
	public class Employee :BaseObject{

		[InverseProperty(nameof(EmployeeTask.AssignedEmployees))]
		public virtual List<EmployeeTask> AssignedEmployeeTasks{ get; set; } = new();
		public virtual  EmployeeDepartment Department { get; set; }
		[RuleRequiredField]
		public virtual string Title { get; set; }
		public virtual EmployeeStatus Status { get; set; }
		public virtual DateTime? HireDate { get; set; }
		[InverseProperty(nameof(EmployeeTask.AssignedEmployee))]
		public virtual List<EmployeeTask> AssignedTasks{ get; set; } = new();
		[InverseProperty(nameof(EmployeeTask.Owner))]
		public virtual List<EmployeeTask> OwnedTasks{ get; set; } = new();
		[InverseProperty(nameof(Evaluation.Employee))]
		public virtual List<Evaluation> Evaluations { get; set; }
		public virtual string PersonalProfile { get; set; }
		public virtual long? ProbationReasonId { get; set; }
		public virtual Probation ProbationReason { get; set; }
		[RuleRequiredField]
		public virtual string FirstName { get; set; }
		[RuleRequiredField]
		public virtual string LastName { get; set; }
		public virtual string FullName { get; set; }
		public virtual PersonPrefix Prefix { get; set; }
		[Attributes.Validation.Phone]
		public virtual string HomePhone { get; set; }
		[RuleRequiredField, Attributes.Validation.Phone]
		public virtual string MobilePhone { get; set; }
		[RuleRequiredField, Attributes.Validation.EmailAddress]
		public virtual string Email { get; set; }
		public virtual string Skype { get; set; }
		public virtual DateTime? BirthDate { get; set; }
		public virtual Picture Picture { get; set; }
		public virtual long? PictureId { get; set; }
		public virtual Address Address{ get; set; } = new();
		public virtual StateEnum AddressState { get; set; }
		public virtual double AddressLatitude { get; set; }
		public virtual double AddressLongitude { get; set; }
		Image _photo;
		[NotMapped]
		public Image Photo {
			get => _photo ??= Picture.CreateImage();
			set {
				if(_photo == value) return;
				_photo?.Dispose();
				_photo = value;
				Picture = value.FromImage();
			}
		}
		bool _unsetFullName;
		public virtual ICollection<Evaluation> EvaluationsCreatedBy { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
		public virtual ICollection<Product> Products { get; set; }
		public virtual ICollection<Product> SupportedProducts { get; set; }
		public virtual ICollection<Quote> Quotes { get; set; }
		public virtual ICollection<CustomerCommunication> Employees { get; set; }
		[NotMapped]
		public string FullNameBindable {
			get => string.IsNullOrEmpty(FullName) || _unsetFullName ? GetFullName() : FullName;
			set{
				_unsetFullName = string.IsNullOrEmpty(value);
				FullName = _unsetFullName ? GetFullName() : value;
			}
		}
		string GetFullName() => $"{FirstName} {LastName}";

		public override string ToString() => FullName;
	}

	public enum EmployeeDepartment {
		Sales = 1, Support, Shipping, Engineering, HumanResources, Management, IT
	}

	public enum EmployeeStatus {
		Salaried, Commission, Contract, Terminated, OnLeave
	}

}