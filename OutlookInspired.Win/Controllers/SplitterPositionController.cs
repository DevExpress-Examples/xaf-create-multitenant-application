using DevExpress.ExpressApp;
using DevExpress.XtraEditors;

namespace OutlookInspired.Win.Controllers{
    public class SplitterPositionController : ViewController<ListView> {
        Control _container;
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            if(View.Model.MasterDetailMode == MasterDetailMode.ListViewAndDetailView) {
                _container = (Control)View.LayoutManager.Container;
                _container.Layout += Container_Layout;
                if(_container is SplitContainerControl control) {
                    control.FixedPanel = SplitFixedPanel.None;
                }
            }
        }

        private void Container_Layout(object sender, LayoutEventArgs e){
            var width = (int)(_container.ClientSize.Width * 0.7);
            switch (_container){
                case SplitContainerControl splitContainerControl:
                    splitContainerControl.SplitterPosition = width;
                    break;
                case SidePanelContainer sidePanelContainer:
                    sidePanelContainer.FixedPanelWidth = width;
                    break;
            }
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            if(_container != null) {
                _container.Layout -= Container_Layout ;
            }
        }
    }
}