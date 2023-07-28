

using System.Collections.ObjectModel;

namespace OutlookInspired.Module.BusinessObjects{
    public class Probation:MigrationBaseObject {
        public  virtual string Reason { get; set; }
        public virtual ObservableCollection<Employee> Employees{ get; set; } = new();
    }
}