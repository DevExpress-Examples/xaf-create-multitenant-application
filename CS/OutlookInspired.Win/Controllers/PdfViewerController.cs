using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Office.Win;
using DevExpress.XtraPdfViewer;

namespace OutlookInspired.Win.Features {
    public class PdfViewerController : ViewController<DetailView> {
        protected override void OnActivated() {
            base.OnActivated();
            View.CustomizeViewItemControl<PdfViewerPropertyEditor>(this, editor => {
                editor.Control.ZoomMode = PdfZoomMode.PageLevel;
                editor.Control.NavigationPaneVisibility = PdfNavigationPaneVisibility.Hidden;
                editor.Control.NavigationPaneInitialVisibility = PdfNavigationPaneVisibility.Hidden;
            });
        }
    }
}
