using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Attributes.Validation;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.BusinessObjects{
	
	[XafDefaultProperty(nameof(FullName))]
	public class Employee :MigrationBaseObject{

		[InverseProperty(nameof(EmployeeTask.AssignedEmployees))]
		public virtual ObservableCollection<EmployeeTask> AssignedEmployeeTasks{ get; set; } 
		public virtual  EmployeeDepartment Department { get; set; }
		[RuleRequiredField]
		// [EditorAlias(EditorAliases.LabelPropertyEditor)]
		[FontSizeDelta(8)]
		public virtual string Title { get; set; }
		[VisibleInListView(false)]
		public virtual EmployeeStatus Status { get; set; }
		[VisibleInListView(false)]
		public virtual DateTime? HireDate { get; set; }
		[InverseProperty(nameof(EmployeeTask.AssignedEmployee))]
		public virtual ObservableCollection<EmployeeTask> AssignedTasks{ get; set; } 
		[InverseProperty(nameof(EmployeeTask.Owner))]
		public virtual ObservableCollection<EmployeeTask> OwnedTasks{ get; set; } 
		[InverseProperty(nameof(Evaluation.Employee))]
		public virtual ObservableCollection<Evaluation> Evaluations { get; set; }
		[VisibleInListView(false)]
		public virtual string PersonalProfile { get; set; }
		[VisibleInListView(false)]
		public virtual Probation ProbationReason { get; set; }
		[RuleRequiredField][VisibleInListView(false)]
		public virtual string FirstName { get; set; }
		[RuleRequiredField][VisibleInListView(false)]
		public virtual string LastName { get; set; }
		[EditorAlias(EditorAliases.LabelPropertyEditor)]
		[FontSizeDelta(16)]
		public virtual string FullName { get; set; }
		
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual PersonPrefix Prefix { get; set; }

		[VisibleInDetailView(false)]
		[XafDisplayName(nameof(Prefix))]
		public virtual Image PrefixImage => Prefix.Image();
		
		[Phone][VisibleInListView(false)]
		public virtual string HomePhone { get; set; }
		[RuleRequiredField, Phone][VisibleInListView(false)]
		public virtual string MobilePhone { get; set; }
		[RuleRequiredField, EmailAddress]
		[EditorAlias(EditorAliases.HyperLinkPropertyEditor)]
		public virtual string Email { get; set; }
		[VisibleInListView(false)]
		public virtual string Skype { get; set; }
		[VisibleInListView(false)]
		public virtual DateTime? BirthDate { get; set; }
		[VisibleInListView(false)]
		public virtual Picture Picture { get; set; }
		public virtual StateEnum State { get; set; }
		[VisibleInListView(false)]
		public virtual double AddressLatitude { get; set; }
		[VisibleInListView(false)]
		public virtual double AddressLongitude { get; set; }
		[RuleRequiredField]
		public virtual string Address { get; set; }
		[RuleRequiredField]
		public virtual string City { get; set; }
		[ZipCode]
		public virtual string ZipCode { get; set; }
		public virtual ICollection<Evaluation> EvaluationsCreatedBy { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
		public virtual ICollection<Product> Products { get; set; }
		public virtual ICollection<Product> SupportedProducts { get; set; }
		public virtual ICollection<Quote> Quotes { get; set; }
		public virtual ICollection<CustomerCommunication> Employees { get; set; }

		[Browsable(false)]
		public virtual Guid? ProbationReasonId{ get; set; }
		
	}

	public enum EmployeeDepartment {
		Sales = 1, Support, Shipping, Engineering, HumanResources, Management, IT
	}

	public enum EmployeeStatus {
		Salaried, Commission, Contract, Terminated, OnLeave
	}

}