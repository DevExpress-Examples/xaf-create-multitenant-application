using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.Controllers.GridListEditor{
    public class NewItemRowHandlingModeController:ObjectViewController<ListView,IViewFilter>{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is DevExpress.ExpressApp.Win.Editors.GridListEditor listEditor&&View.ObjectTypeInfo.FindAttribute<NewItemRowHandlingModeAttribute>()!=null){
                listEditor.NewItemRowHandlingMode=GridListEditorNewItemRowHandlingMode.NativeControl;
            }
        }
    }
}