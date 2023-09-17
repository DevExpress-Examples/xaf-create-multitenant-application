using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using OutlookInspired.Module.Controllers;

namespace OutlookInspired.Win.Controllers{
    
    public class SplitterPositionController : Module.Controllers.SplitterPositionController {
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
            var width = _container.ClientSize.Width * ((IModelListViewSplitterRelativePosition)View.Model.SplitLayout).RelativePosition / 100;
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