using System.ComponentModel;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers{
    
    public interface IModelDashboardViewMasterDetail{
        [Category(OutlookInspiredModule.ModelCategory)]
        bool MasterDetail{ get; set; }
    }

    public class DashboardMasterDetailController:ViewController<DashboardView>,IModelExtender{
        private readonly SimpleAction _processMasterViewSelectedObjectAction;
        private NestedFrame _masterFrame;
        private NestedFrame _childFrame;

        public DashboardMasterDetailController(){
            _processMasterViewSelectedObjectAction = new SimpleAction(this,"ProcessMasterViewSelectedObject",PredefinedCategory.ListView);
            _processMasterViewSelectedObjectAction.Executed+=(_, e) 
                => e.ShowViewParameters.CreatedView = Application.NewDetailView(e.Action.SelectionContext.CurrentObject);
        }

        protected override void OnActivated(){
            base.OnActivated();
            if (((IModelDashboardViewMasterDetail)View.Model).MasterDetail){
                //see ApplicationOnDetailViewCreating comments 
                Application.DetailViewCreating+=ApplicationOnDetailViewCreating;
            }
        }

        protected override void OnDeactivated(){
            base.OnDeactivated();
            Application.DetailViewCreating-=ApplicationOnDetailViewCreating;
        }

        //The NewObject action will create a new detailview out of the current view, which is the one with the UserControl, but we want the default detailview
        private void ApplicationOnDetailViewCreating(object sender, DetailViewCreatingEventArgs e){
            if (e.ViewID == _masterFrame?.View?.Id){
                e.ViewID = Application.FindDetailViewId(_masterFrame!.View!.ObjectTypeInfo.Type);
            }
        }

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (((IModelDashboardViewMasterDetail)View.Model).MasterDetail){
                _masterFrame = View.MasterFrame();
                _childFrame = View.NestedFrames(ViewType.DetailView).First(nestedFrame => nestedFrame != _masterFrame);
                var controlViewItem = _masterFrame.View.ToCompositeView().GetItems<ControlViewItem>().FirstOrDefault();
                if (controlViewItem != null){
                    controlViewItem.ControlCreated+=ControlViewItemOnControlCreated;
                }
                else{
                    _masterFrame.View.SelectionChanged += (_, _) => _childFrame.View.SetCurrentObject(_masterFrame.View.CurrentObject);
                }
            }
        }

        //handle UserControl events to synchronize the views
        //if an native ObjectType action e.g. DeleteAction does an ObjectSpace.Commit, we refresh UserControl
        private void ControlViewItemOnControlCreated(object sender, EventArgs e){
            var userControl = (IUserControl)((ControlViewItem)sender).Control;
            _masterFrame.ActiveActions().ForEach(action => action.SelectionContext = userControl);
            userControl.CurrentObjectChanged += UserControlOnCurrentObjectChanged;
            userControl.ProcessObject+=UserControlOnProcessObject;
            _masterFrame.View.ObjectSpace.Committed += (_, _) => userControl.Refresh();
        }

        // assign the UserControl as the SelectionContext of the ProcessMasterViewSelectedObject so it knows the CurrentObject of the UserControl
        private void UserControlOnProcessObject(object sender, EventArgs e){
            var userControl = (IUserControl)sender;
            _processMasterViewSelectedObjectAction.SelectionContext = userControl;
            _processMasterViewSelectedObjectAction.DoExecute();
        }

        //set the MasterFrame CurrentObject for Actions to be aware
        //set the ChildFrame CurrentObject so it can display it
        //if the ChildFrame has a UserControl refresh it
        private void UserControlOnCurrentObjectChanged(object sender, EventArgs e){
            var userControl = (IUserControl)sender;
            _masterFrame.View.SetCurrentObject(userControl.CurrentObject);
            _childFrame.View.SetCurrentObject(userControl.CurrentObject);
            _childFrame.View.ToCompositeView().GetItems<ControlViewItem>()
                .Select(item => item.Control).OfType<IUserControl>()
                .ForEach(control => control.Refresh(_childFrame.View.CurrentObject));
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) 
            => extenders.Add<IModelDashboardView, IModelDashboardViewMasterDetail>();
    }
}