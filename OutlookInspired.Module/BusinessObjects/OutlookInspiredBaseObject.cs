

using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.BaseImpl.EF;

namespace OutlookInspired.Module.BusinessObjects{
    public interface IOutlookInspiredBaseObject{
        long IdInt64{ get; set; }
    }

    [Appearance("Hide ShowInDocument",AppearanceItemType.Action, "1=1",TargetItems = "ShowInDocument",Visibility = ViewItemVisibility.Hide,Context = "Any;Employee_ListView;"+Employee.LayoutViewDetailView)]
    public abstract class OutlookInspiredBaseObject:BaseObject, IOutlookInspiredBaseObject{
        [Browsable(false)]
        public virtual long IdInt64{ get; set; }

        [NotMapped][Browsable(false)]
        public new IObjectSpace ObjectSpace{
            get => base.ObjectSpace;
            set => base.ObjectSpace=value;
        }

    }
}