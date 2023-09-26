using System.ComponentModel;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.Services;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Module.Features.MasterDetail{
    public interface IModelDashboardViewMasterDetail{
        [Category(OutlookInspiredModule.ModelCategory)]
        bool MasterDetail{ get; set; }
    }
    
    public class MasterDetailController:ViewController<DashboardView>,IModelExtender{
        private readonly SimpleAction _processMasterViewSelectedObjectAction;
        private NestedFrame _masterFrame;
        private NestedFrame _childFrame;
        private ControlViewItem _controlViewItem;
        private IUserControl _userControl;

        public MasterDetailController(){
            _processMasterViewSelectedObjectAction = new SimpleAction(this,"ProcessMasterViewSelectedObject",PredefinedCategory.ListView);
            _processMasterViewSelectedObjectAction.Executed+=(_, e) 
                => e.ShowViewParameters.CreatedView = Application.NewDetailView(e.Action.SelectionContext.CurrentObject);
        }

        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (!((IModelDashboardViewMasterDetail)View.Model).MasterDetail)return;
            if (_userControl != null){
                _controlViewItem.ControlCreated-=ControlViewItemOnControlCreated;
                _userControl.CurrentObjectChanged-=UserControlOnCurrentObjectChanged;
                _userControl.ProcessObject-=UserControlOnProcessObject;
                _masterFrame.View.ObjectSpace.Committed-=ObjectSpaceOnCommitted;
            }
            else{
                _masterFrame.View.SelectionChanged-=ViewOnSelectionChanged;
            }
        }

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            
            if (!((IModelDashboardViewMasterDetail)View.Model).MasterDetail)return;
            _masterFrame = View.MasterFrame();
            _masterFrame.GetController<NewObjectViewController>().UseObjectDefaultDetailView();
            _childFrame = View.ChildFrame();
            _controlViewItem = _masterFrame.View.ToCompositeView().GetItems<ControlViewItem>().FirstOrDefault();
            if (_controlViewItem != null){
                _controlViewItem.ControlCreated+=ControlViewItemOnControlCreated;
            }
            else{
                _masterFrame.View.SelectionChanged += ViewOnSelectionChanged;
            }
        }

        private void ViewOnSelectionChanged(object sender, EventArgs e) 
            => _childFrame.View.SetCurrentObject(_masterFrame.View.CurrentObject);

        private void ControlViewItemOnControlCreated(object sender, EventArgs e){
            _userControl = (IUserControl)((ControlViewItem)sender).Control;
            _masterFrame.ActiveActions().ForEach(action => action.SelectionContext = _userControl);
            _userControl.CurrentObjectChanged += UserControlOnCurrentObjectChanged;
            _userControl.ProcessObject+=UserControlOnProcessObject;
            _masterFrame.View.ObjectSpace.Committed += ObjectSpaceOnCommitted;
        }

        private void ObjectSpaceOnCommitted(object sender, EventArgs e) => _userControl.Refresh();

        private void UserControlOnProcessObject(object sender, EventArgs e){
            var userControl = (IUserControl)sender;
            _processMasterViewSelectedObjectAction.SelectionContext = userControl;
            _processMasterViewSelectedObjectAction.DoExecute();
        }
        
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