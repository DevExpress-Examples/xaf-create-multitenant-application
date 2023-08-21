using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.Editors;
using OutlookInspired.Module.Attributes;

namespace OutlookInspired.Win.Controllers.GridListEditor{
    public class NewItemRowHandlingModeController:ListViewControllerBase{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is DevExpress.ExpressApp.Win.Editors.GridListEditor listEditor&&View.ObjectTypeInfo.FindAttribute<NewItemRowHandlingModeAttribute>()!=null){
                listEditor.NewItemRowHandlingMode=GridListEditorNewItemRowHandlingMode.NativeControl;
            }
        }
    }
}