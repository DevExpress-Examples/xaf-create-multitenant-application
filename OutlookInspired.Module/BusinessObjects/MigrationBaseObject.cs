

using System.ComponentModel;
using DevExpress.Persistent.BaseImpl.EF;

namespace OutlookInspired.Module.BusinessObjects{
    public abstract class MigrationBaseObject:BaseObject{
        [Browsable(false)]
        public virtual long IdInt64{ get; set; }
    }
}