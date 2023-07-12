namespace OutlookInspired.Module.BusinessObjects{
    public class TaskAttachedFile :MyBaseObject{
        public virtual EmployeeTask EmployeeTask { get; set; }
        public  virtual long? EmployeeTaskId { get; set; }
        public  virtual string Name { get; set; }
        public  virtual byte[] Content { get; set; }
    }
}