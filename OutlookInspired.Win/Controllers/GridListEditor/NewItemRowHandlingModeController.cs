using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using ViewFilter = OutlookInspired.Module.BusinessObjects.ViewFilter;

namespace OutlookInspired.Win.Controllers.GridListEditor{
    public class NewItemRowHandlingModeController:ObjectViewController<ListView,ViewFilter>{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is DevExpress.ExpressApp.Win.Editors.GridListEditor listEditor){
                listEditor.NewItemRowHandlingMode=GridListEditorNewItemRowHandlingMode.NativeControl;
            }
        }
    }
}