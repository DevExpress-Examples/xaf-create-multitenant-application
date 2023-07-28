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
	
	[DefaultProperty(nameof(FullName))]
	[VisibleInReports]
	public class Employee :MigrationBaseObject{

		[InverseProperty(nameof(EmployeeTask.AssignedEmployees))][Aggregated]
		public virtual ObservableCollection<EmployeeTask> AssignedEmployeeTasks{ get; set; } = new(); 
		public virtual  EmployeeDepartment Department { get; set; }
		[RuleRequiredField]
		
		[FontSizeDelta(8)]
		public virtual string Title { get; set; }
		[VisibleInListView(false)]
		public virtual EmployeeStatus Status { get; set; }
		[VisibleInListView(false)]
		public virtual DateTime? HireDate { get; set; }

		[InverseProperty(nameof(EmployeeTask.AssignedEmployee))][Aggregated]
		public virtual ObservableCollection<EmployeeTask> AssignedTasks{ get; set; } = new();

		[InverseProperty(nameof(EmployeeTask.Owner))][Aggregated]
		public virtual ObservableCollection<EmployeeTask> OwnedTasks{ get; set; } = new(); 
		[InverseProperty(nameof(Evaluation.Employee))][Aggregated]
		public virtual ObservableCollection<Evaluation> Evaluations { get; set; }=new();
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

		[Aggregated]
		public virtual ObservableCollection<Evaluation> EvaluationsCreatedBy{ get; set; } = new();
		[Aggregated]
		public virtual ObservableCollection<Order> Orders{ get; set; } = new();
		[Aggregated]
		public virtual ObservableCollection<Product> Products{ get; set; } = new();
		[Aggregated]
		public virtual ObservableCollection<Product> SupportedProducts{ get; set; } = new();
		[Aggregated]
		public virtual ObservableCollection<Quote> Quotes{ get; set; } = new();
		[Aggregated]
		public virtual ObservableCollection<CustomerCommunication> Employees{ get; set; } = new();

		[Browsable(false)]
		public virtual Guid? ProbationReasonId{ get; set; }

		public override void OnSaving(){
            if (ObjectSpace.IsObjectToDelete(this)){
				ObjectSpace.Delete(Products);
				ObjectSpace.Delete(SupportedProducts);	
				ObjectSpace.Delete(AssignedTasks);	
				ObjectSpace.Delete(OwnedTasks);	
			}
		}
	}

	public enum EmployeeDepartment {
		Sales = 1, Support, Shipping, Engineering, HumanResources, Management, IT
	}

	public enum EmployeeStatus {
		Salaried, Commission, Contract, Terminated, OnLeave
	}

}