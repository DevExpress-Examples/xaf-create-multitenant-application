using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.BusinessObjects{
    [Appearance(nameof(DueDate),AppearanceItemType.ViewItem, "1=1",TargetItems = nameof(DueDate),FontStyle = FontStyle.Bold)]
    public class EmployeeTask:MigrationBaseObject{
        [Aggregated]
        public virtual ObservableCollection<Employee> AssignedEmployees{ get; set; } = new();
        [RuleRequiredField]
        [FontSizeDelta(8)]
        public virtual string Subject { get; set; }
        public virtual string Description { get; set; }
        public virtual string RtfTextDescription { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? DueDate { get; set; }
        public virtual EmployeeTaskStatus Status { get; set; }
        [VisibleInListView(false)][VisibleInLookupListView(false)]
        public virtual EmployeeTaskPriority Priority { get; set; }

        [VisibleInDetailView(false)][XafDisplayName(nameof(Priority))]
        public Image PriorityImage => Priority.Image();
        
        [EditorAlias(EditorAliases.ProgressEditor)]
        public virtual int Completion { get; set; }
        public virtual bool Reminder { get; set; }
        public virtual DateTime? ReminderDateTime { get; set; }

        public virtual Employee AssignedEmployee { get; set; }
        [Browsable(false)]
        public virtual Guid? AssignedEmployeeId { get; set; }
        public virtual Employee Owner { get; set; }
        [Browsable(false)]
        public virtual Guid? OwnerId { get; set; }
        public virtual CustomerEmployee CustomerEmployee { get; set; }
        public virtual  EmployeeTaskFollowUp FollowUp { get; set; }
        public  virtual bool Private { get; set; }
        public  virtual string Category { get; set; }

        [Aggregated]
        public virtual ObservableCollection<TaskAttachedFile> AttachedFiles{ get; set; } = new();
        public  virtual bool AttachedCollectionsChanged { get; set; }
        public  virtual long? ParentId { get; set; }
        public  virtual string Predecessors { get; set; }
        public override string ToString() => $"{Subject} - {Description}, due {DueDate}, {Status},\r\nOwner: {Owner}";
        public bool Overdue 
            => Status != EmployeeTaskStatus.Completed && DueDate.HasValue && DateTime.Now >= DueDate.Value.Date.AddDays(1);
        public int AttachedFilesCount => AttachedFiles?.Count ?? 0;
        public string AssignedEmployeesFullList => AssignedEmployees == null ? "" : string.Join(", ", AssignedEmployees.Select(x => x.FullName));
    }

    public enum EmployeeTaskStatus {
        NotStarted, Completed, InProgress, NeedAssistance, Deferred
    }
    public enum EmployeeTaskPriority {
        [ImageName("PriorityLow")]
        Low,
        [ImageName("PriorityNormal")]
        Normal,
        [ImageName("PriorityHigh")]
        High,
        [ImageName("PriorityUrgent")]
        Urgent
    }

    public enum EmployeeTaskFollowUp {
        Today, Tomorrow, ThisWeek, NextWeek, NoDate, Custom
    }

}