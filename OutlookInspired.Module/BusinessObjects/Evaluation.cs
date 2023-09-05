using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using OutlookInspired.Module.Attributes;

namespace OutlookInspired.Module.BusinessObjects{
    [Appearance(nameof(StartOn),AppearanceItemType.ViewItem, "1=1",TargetItems = nameof(StartOn),FontStyle = FontStyle.Bold,Context = "Employee_Evaluations_ListView")]
    [Appearance(nameof(Manager),AppearanceItemType.ViewItem, "1=1",TargetItems = nameof(Manager),FontStyle = FontStyle.Bold,Context = EmployeeEvaluationsChildListView)]
    [Appearance(nameof(StartOn)+"_"+EmployeeEvaluationsChildListView,AppearanceItemType.ViewItem, "1=1",TargetItems = nameof(StartOn),FontColor = "Blue",Context = EmployeeEvaluationsChildListView)]
    [Appearance(nameof(Rating),AppearanceItemType.ViewItem, nameof(Rating)+"='"+nameof(EvaluationRating.Good)+"'",TargetItems = "*",FontColor = "Green",Context = "Employee_Evaluations_ListView")]
    [CloneView(CloneViewType.ListView, EmployeeEvaluationsChildListView)]
    [DefaultClassOptions][ImageName("EvaluationYes")][VisibleInReports(false)]
    public class Evaluation :OutlookInspiredBaseObject,IEvent{
        public const string EmployeeEvaluationsChildListView="Employee_Evaluations_ListView_Child";
	    private const int NoneReminder = -1;
		
		private int _remindInSeconds = NoneReminder;

		public override void OnCreated() {
			base.OnCreated();
			StartOn = DateTime.Now;
			EndOn = StartOn.Value.AddHours(1);
			Color = Color.White;
		}

		[FieldSize(FieldSizeAttribute.Unlimited)]
		public virtual string Description{ get; set; }
		public virtual DateTime? EndOn { get; set; }
		[ImmediatePostData][Browsable(false)]
		public virtual Boolean AllDay { get; set; }
		[Browsable(false)]
		public virtual String Location { get; set; }
		[Browsable(false)]
		public virtual Int32 Label { get; set; }
		[Browsable(false)]
		public virtual Int32 Status { get; set; }
		[Browsable(false)]
		public virtual Int32 Type { get; set; }
		
		[NotMapped, Browsable(false)]
		public virtual String ResourceId{ get; set; }

		[Browsable(false)]
		public Object AppointmentId => ID;

		[Browsable(false)]
		[RuleFromBoolProperty("EvaluationIntervalValid", DefaultContexts.Save, "The start date must be less than the end date", SkipNullOrEmptyValues = false, UsedProperties = "StartOn, EndOn")]
		public Boolean IsIntervalValid => StartOn <= EndOn;
		
		DateTime IEvent.StartOn {
			get => StartOn ?? DateTime.MinValue;
			set => StartOn = value;
		}
		DateTime IEvent.EndOn {
			get => EndOn ?? DateTime.MinValue;
			set => EndOn = value;
		}

		[NonCloneable]
		[Browsable(false)]
		[StringLength(200)]
		public virtual string ReminderInfoXml{ get; set; }

		[Browsable(false)]
		[NotMapped]
		public TimeSpan? RemindIn {
			get => _remindInSeconds < 0 ? null : TimeSpan.FromSeconds(_remindInSeconds);
			set => _remindInSeconds = value.HasValue ? (int)value.Value.TotalSeconds : NoneReminder;
		}
		[Browsable(false)]
		public virtual int RemindInSeconds { get; set; }
		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		public string NotificationMessage => Subject;

		[Browsable(false)]
		public object UniqueId => ID;

		[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
		public virtual bool IsPostponed { get; set; }
		
		[RuleRequiredField]
        public virtual Employee Manager{ get; set; }
        [Browsable(false)]
        public virtual Guid? ManagerId{ get; set; }
        [RuleRequiredField]
        public virtual DateTime? StartOn{ get; set; }
        // [RuleRequiredField(DefaultContexts.Save)]
        public virtual Employee Employee{ get; set; }
        [FontSizeDelta(8)]
        public virtual string Subject{ get; set; }
        
        public virtual EvaluationRating Rating{ get; set; }

        [VisibleInListView(false)]
        public virtual Raise Raise{ get; set; }

        [VisibleInListView(false)]
        public virtual Bonus Bonus{ get; set; }
        
        [Browsable(false)]
        public virtual Int32 ColorInt { get; protected set; }
        
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public Object Id => ID;

        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public Int32 OleColor => ColorTranslator.ToOle(Color.FromArgb(ColorInt));

        [NotMapped][Browsable(false)]
        public Color Color {
            get => Color.FromArgb(ColorInt);
            set => ColorInt = value.ToArgb();
        }
    }

    public enum Raise{
        [XafDisplayName("RAISE")]
        [ImageName("EvaluationNo")]
        No,
        [XafDisplayName("RAISE")]
        [ImageName("EvaluationYes")]
        Yes
    }
    public enum Bonus{
        [XafDisplayName("BONUS")]
        [ImageName("EvaluationNo")]
        No,
        [XafDisplayName("BONUS")]
        [ImageName("EvaluationYes")]
        Yes
    }

    public enum EvaluationRating {
        Unset, Good, Average, Poor
    }

}