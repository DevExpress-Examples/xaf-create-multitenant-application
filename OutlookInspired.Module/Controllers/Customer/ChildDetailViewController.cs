using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;

namespace OutlookInspired.Module.Controllers.Customer{
    public class ChildDetailViewController:ObjectViewController<DetailView,BusinessObjects.Customer>{
        public ChildDetailViewController() => TargetViewId = BusinessObjects.Customer.ChildDetailViewId;
        protected override void OnActivated(){
            base.OnActivated();
            View.CurrentObjectChanged+=ViewOnCurrentObjectChanged;
            // View.GetItems<ControlViewItem>().First().ControlCreated+=(sender, args) => ((ControlViewItem)sender).Control
        }

        protected override void OnDeactivated(){
            base.OnDeactivated();
            View.CurrentObjectChanged-=ViewOnCurrentObjectChanged;
        }

        private void ViewOnCurrentObjectChanged(object sender, EventArgs e){
            
        }
    }
}