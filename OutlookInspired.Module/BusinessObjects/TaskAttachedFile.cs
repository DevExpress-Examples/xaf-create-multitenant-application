using System.ComponentModel;


namespace OutlookInspired.Module.BusinessObjects{
    public class TaskAttachedFile :MigrationBaseObject{
        public virtual EmployeeTask EmployeeTask { get; set; }
        public  virtual string Name { get; set; }
        public  virtual byte[] Content { get; set; }
        [Browsable(false)]
        public virtual Guid? EmployeeTaskId{ get; set; }
    }
}