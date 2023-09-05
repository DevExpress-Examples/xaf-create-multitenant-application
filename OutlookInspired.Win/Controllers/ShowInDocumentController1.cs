using DevExpress.ExpressApp.Office.Win;

namespace OutlookInspired.Win.Controllers{
    
    public class ShowInDocumentController1:ShowInDocumentController{
        public ShowInDocumentController1(){
            
        }

        [Obsolete]
        protected override void OnActivated(){
            // Active[nameof(ShowInDocumentController1)] = false;
            base.OnActivated();
        }
    }
}