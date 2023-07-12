namespace OutlookInspired.Module.BusinessObjects{
    public class Probation:BaseObject {
        public  virtual string Reason { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}