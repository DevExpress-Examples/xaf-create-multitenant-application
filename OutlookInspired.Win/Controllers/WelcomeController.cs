using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Office.Win;
using DevExpress.XtraRichEdit;

namespace OutlookInspired.Win.Controllers{
    public class WelcomeController:Module.Features.WelcomeController{
        protected override void OnActivated(){
            base.OnActivated();
            View.CustomizeViewItemControl<RichTextPropertyEditor>(this, editor => editor.RichEditControl.ActiveViewType=RichEditViewType.PrintLayout);
        }
    }
}