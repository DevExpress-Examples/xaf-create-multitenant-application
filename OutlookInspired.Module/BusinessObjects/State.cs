using System.ComponentModel;


namespace OutlookInspired.Module.BusinessObjects{
    [DefaultProperty(nameof(LongName))]
    public class State:MigrationBaseObject{
        public virtual string LongName{ get; set; }
        public virtual StateEnum ShortName{ get; set; }
        public virtual byte[] SmallFlag{ get; set; }
        public virtual byte[] LargeFlag{ get; set; }
        
    }
}