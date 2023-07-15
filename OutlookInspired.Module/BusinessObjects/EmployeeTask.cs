using System.Collections.ObjectModel;

using DevExpress.Persistent.Validation;

namespace OutlookInspired.Module.BusinessObjects{
    public class EmployeeTask:MigrationBaseObject{
        public virtual ObservableCollection<Employee> AssignedEmployees{ get; set; } = new();
        [RuleRequiredField]
        public virtual string Subject { get; set; }
        public virtual string Description { get; set; }
        public virtual string RtfTextDescription { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? DueDate { get; set; }
        public virtual EmployeeTaskStatus Status { get; set; }
        public virtual EmployeeTaskPriority Priority { get; set; }
        public virtual int Completion { get; set; }
        public virtual bool Reminder { get; set; }
        public virtual DateTime? ReminderDateTime { get; set; }
        public virtual Employee AssignedEmployee { get; set; }
        public virtual Employee Owner { get; set; }
        public virtual CustomerEmployee CustomerEmployee { get; set; }
        public virtual  EmployeeTaskFollowUp FollowUp { get; set; }
        public  virtual bool Private { get; set; }
        public  virtual string Category { get; set; }
        public virtual ObservableCollection<TaskAttachedFile> AttachedFiles { get; set; }
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
        Low, Normal, High, Urgent
    }

    public enum EmployeeTaskFollowUp {
        Today, Tomorrow, ThisWeek, NextWeek, NoDate, Custom
    }

}