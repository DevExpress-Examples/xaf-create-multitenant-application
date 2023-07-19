

using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.EF;

namespace OutlookInspired.Module.BusinessObjects{
    public abstract class MigrationBaseObject:BaseObject{
        [Browsable(false)]
        public virtual long IdInt64{ get; set; }

        [NotMapped]
        public new IObjectSpace ObjectSpace{
            get => base.ObjectSpace;
            set => base.ObjectSpace=value;
        }
    }
}