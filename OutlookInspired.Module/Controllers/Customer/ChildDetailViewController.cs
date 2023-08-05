using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.SystemModule;

namespace OutlookInspired.Module.Controllers.Customer{
    public class ChildDetailViewController:ObjectViewController<DetailView,BusinessObjects.Customer>{
        public ChildDetailViewController() => TargetViewId="CustomerGridView_DetailView";
        protected override void OnActivated(){
            base.OnActivated();
            var deleteObjectsViewController = Frame.GetController<DeleteObjectsViewController>();
            deleteObjectsViewController.DeleteAction.Enabled.ResultValueChanged += (sender, args) => {

            };
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