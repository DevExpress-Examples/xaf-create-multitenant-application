using DevExpress.Persistent.Base;

namespace OutlookInspired.Module.BusinessObjects{
    [ImageName("ProductQuickComparisons")]
    public class CustomerCommunication:OutlookInspiredBaseObject {
        public virtual Employee Employee { get; set; }
        public virtual CustomerEmployee CustomerEmployee { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Type { get; set; }
        public virtual string Purpose { get; set; }
    }
}