namespace OutlookInspired.Module.BusinessObjects{
    public class CustomerCommunication:MyBaseObject {
        public virtual Employee Employee { get; set; }
        public virtual long? EmployeeId { get; set; }
        public virtual CustomerEmployee CustomerEmployee { get; set; }
        public virtual  long? CustomerEmployeeId { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Type { get; set; }
        public virtual string Purpose { get; set; }
    }
}