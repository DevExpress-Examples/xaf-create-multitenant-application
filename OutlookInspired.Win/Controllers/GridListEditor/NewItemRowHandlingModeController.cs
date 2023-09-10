using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using OutlookInspired.Module.Attributes;
using ViewFilter = OutlookInspired.Module.BusinessObjects.ViewFilter;

namespace OutlookInspired.Win.Controllers.GridListEditor{
    public class NewItemRowHandlingModeController:ObjectViewController<ListView,ViewFilter>{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is DevExpress.ExpressApp.Win.Editors.GridListEditor listEditor&&View.ObjectTypeInfo.FindAttribute<NewItemRowHandlingModeAttribute>()!=null){
                listEditor.NewItemRowHandlingMode=GridListEditorNewItemRowHandlingMode.NativeControl;
            }
        }
    }
}