

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace OutlookInspired.Module.BusinessObjects{
    public class Probation:OutlookInspiredBaseObject {
        [MaxLength(100)]
        public  virtual string Reason { get; set; }
        public virtual ObservableCollection<Employee> Employees{ get; set; } = new();
    }
}