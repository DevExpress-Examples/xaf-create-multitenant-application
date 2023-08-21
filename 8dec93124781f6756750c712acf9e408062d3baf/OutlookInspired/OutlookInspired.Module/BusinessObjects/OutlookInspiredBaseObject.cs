

using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.EF;

namespace OutlookInspired.Module.BusinessObjects{
    public interface IOutlookInspiredBaseObject{
        long IdInt64{ get; set; }
    }

    public abstract class OutlookInspiredBaseObject:BaseObject, IOutlookInspiredBaseObject{
        [Browsable(false)]
        public virtual long IdInt64{ get; set; }

        [NotMapped][Browsable(false)]
        public new IObjectSpace ObjectSpace{
            get => base.ObjectSpace;
            set => base.ObjectSpace=value;
        }

        [Obsolete("Default property T1182433 ")]
        public override string ToString(){
            return base.ToString();
        }
    }
}