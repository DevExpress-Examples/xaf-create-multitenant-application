using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;


namespace OutlookInspired.Module.BusinessObjects{
    public class TaskAttachedFile :OutlookInspiredBaseObject{
        [RuleRequiredField]
        public virtual EmployeeTask EmployeeTask { get; set; }
        [ExpandObjectMembers(ExpandObjectMembers.Never), RuleRequiredField()]
        public virtual FileData File { get; set; }
        [Browsable(false)]
        public virtual Guid? EmployeeTaskId{ get; set; }
    }
}