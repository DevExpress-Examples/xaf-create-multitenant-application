using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Attributes.Appearance;
using OutlookInspired.Module.Attributes.Validation;
using OutlookInspired.Module.Features.CloneView;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Features.ViewFilter;
using OutlookInspired.Module.Services;
using EditorAliases = OutlookInspired.Module.Services.EditorAliases;

namespace OutlookInspired.Module.BusinessObjects{
	
	[DefaultProperty(nameof(FullName))]
	[ImageName("BO_Person")]
	[CloneView(CloneViewType.DetailView, LayoutViewDetailView)]
	[CloneView(CloneViewType.DetailView, ChildDetailView)]
	[CloneView(CloneViewType.DetailView, MapsDetailView)]
	[VisibleInReports(true)]
	[ForbidDelete()]
	public class Employee :OutlookInspiredBaseObject,IViewFilter,IObjectSpaceLink,IResource,ITravelModeMapsMarker{
		public const string MapsDetailView = "Employee_DetailView_Maps";
		public const string ChildDetailView = "Employee_DetailView_Child";
		public const string LayoutViewDetailView = "EmployeeLayoutView_DetailView";
		
		[NotMapped][Browsable(false)]
		public new IObjectSpace ObjectSpace{ get; set; }
		[VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
		public object Id => ID;

		[Browsable(false)]
		public Int32 OleColor => 0;
		public virtual  EmployeeDepartment Department { get; set; }
		[RuleRequiredField][FontSizeDelta(8)]
		public virtual string Title { get; set; }
		double IBaseMapsMarker.Latitude => AddressLatitude;
		double IBaseMapsMarker.Longitude => AddressLongitude;

		[VisibleInListView(false)]
		public virtual EmployeeStatus Status { get; set; }
		[VisibleInListView(false)]
		public virtual DateTime? HireDate { get; set; }

		[InverseProperty(nameof(EmployeeTask.AssignedEmployee))][Aggregated]
		public virtual ObservableCollection<EmployeeTask> AssignedTasks{ get; set; } = new();
		
		[InverseProperty(nameof(EmployeeTask.AssignedEmployees))]
		public virtual ObservableCollection<EmployeeTask> AssignedEmployeeTasks{ get; set; } = new();
		[InverseProperty(nameof(EmployeeTask.Owner))]
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
		
		[FontSizeDelta(16)]
		public virtual string FullName { get; set; }
		
		[VisibleInListView(false)][VisibleInLookupListView(false)]
		public virtual PersonPrefix Prefix { get; set; }

		[VisibleInDetailView(false)]
		[XafDisplayName(nameof(Prefix))]
		public virtual byte[] PrefixImage => Prefix.ImageInfo().ImageBytes;
		
		[Phone][VisibleInListView(false)]
		public virtual string HomePhone { get; set; }
		[RuleRequiredField, Phone][VisibleInListView(false)]
		public virtual string MobilePhone { get; set; }
		[RuleRequiredField, EmailAddress]
		[EditorAlias(EditorAliases.HyperLinkPropertyEditor)]
		public virtual string Email { get; set; }

		[VisibleInDetailView(false)]
		[NotMapped]
		public virtual ObservableCollection<RoutePoint> RoutePoints{ get; set; } = new();
		[VisibleInListView(false)]
		public virtual string Skype { get; set; }
		[VisibleInListView(false)]
		public virtual DateTime? BirthDate { get; set; }
		[VisibleInListView(false)]
		public virtual Picture Picture { get; set; }
		[NotMapped][VisibleInListView(false)][VisibleInDetailView(false)][VisibleInLookupListView(false)][XafDisplayName("A")]
		public string AAddress{ get; set; }
		[NotMapped][VisibleInListView(false)][VisibleInDetailView(false)][VisibleInLookupListView(false)][XafDisplayName("B")]
		public string BAddress{ get; set; }
		public virtual StateEnum State { get; set; }
		[VisibleInListView(false)]
		public virtual double AddressLatitude { get; set; }
		[VisibleInListView(false)]
		public virtual double AddressLongitude { get; set; }
		[RuleRequiredField][FontSizeDelta(2)]
		public virtual string Address { get; set; }
		[RuleRequiredField]
		public virtual string City { get; set; }
        [ZipCode]
		public virtual string ZipCode { get; set; }
		public virtual ObservableCollection<Evaluation> EvaluationsCreatedBy{ get; set; } = new();
		public virtual ObservableCollection<Order> Orders{ get; set; } = new();
		public virtual ObservableCollection<Product> Products{ get; set; } = new();
		public virtual ObservableCollection<Product> SupportedProducts{ get; set; } = new();
		public virtual ObservableCollection<Quote> Quotes{ get; set; } = new();
		public virtual ObservableCollection<CustomerCommunication> CustomerCommunications{ get; set; } = new();
		[Browsable(false)]
		public virtual Guid? ProbationReasonId{ get; set; }

		public override void OnCreated(){
			base.OnCreated();
			// Roles.Add(ObjectSpace.EnsureDefaultRole());
		}

		public override void OnSaving() => UpdateRole();

		private void UpdateRole(){
			// if (Roles.Any(role => role.Name == Department.ToString())) return;
			// Roles.Where(role => role.Name != "Default").ToArray()
				// .Do(role => Roles.Remove(role)).Enumerate();
			// Roles.Add(ObjectSpace.FindRole(Department));
		}

		[NotMapped]
		public virtual string Caption{
			get => FullName;
			set => FullName=value;
		}

		[NotMapped][VisibleInListView(false)][VisibleInDetailView(false)][VisibleInLookupListView(false)]
		[FontSizeDelta(2)]
		public virtual string RouteResult{ get; set; }

		[Browsable(false)][RuleRequiredField]
		public virtual ApplicationUser User{ get; set; }
	}

	public enum EmployeeDepartment {
		[ImageName(nameof(Sales))]
		Sales = 1, 
		[ImageName(nameof(Support))]
		Support,
		[ImageName("ProductQuickShippments")]
		Shipping,
		[ImageName("FunctionsEngineering")]
		Engineering,
		[ImageName("GroupByResource")]
		HumanResources,
		[ImageName("ManageRelations")]
		Management,
		[ImageName("Actions_Info")]
		IT
	}

	public enum EmployeeStatus {
		[ImageName("Salaried")]
		Salaried,
		[ImageName("Commission")]
		Commission,
		[ImageName("Contract")]
		Contract,
		[ImageName("Terminated")]
		Terminated,
		[ImageName("OnLeave")]
		OnLeave
	}

}