using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;

namespace OutlookInspired.Module.Model{
    public class UserControlViewItem:ControlViewItem{
        public UserControlViewItem(string controlTypeName, string id, string caption, Type objectType) : base(controlTypeName, id, caption, objectType){
        }

        public UserControlViewItem(string id, string caption, object control) : base(id, caption, control){
        }

        public UserControlViewItem(string id, object control) : base(id, control){
        }

        public UserControlViewItem(IModelControlDetailItem model, Type objectType) : base(model, objectType){
        }

        protected override object CreateControlCore(){
            return base.CreateControlCore();
        }
    }
}