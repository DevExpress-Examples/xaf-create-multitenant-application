using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Office.Blazor.Editors;
using ViewType = DevExpress.Blazor.RichEdit.ViewType;

namespace OutlookInspired.Blazor.Server.Features{
    public class RichTextPropertyEditorController:ViewController<DetailView>{
        protected override void OnActivated(){
            base.OnActivated();
            View.CustomizeViewItemControl<RichTextPropertyEditor>(this, editor => editor.ComponentModel.ViewType=ViewType.Simple);
        }

    }
}