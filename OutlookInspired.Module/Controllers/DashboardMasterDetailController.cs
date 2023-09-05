using System.ComponentModel;
using System.Linq.Expressions;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
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
            Active[nameof(IModelDashboardViewMasterDetail.MasterDetail)] = ((IModelDashboardViewMasterDetail)View.Model).MasterDetail;
        }

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            _masterFrame = View.MasterFrame();
            _masterFrame.GetController<NewObjectViewController>().UseObjectDefaultDetailView();
            _childFrame = View.ChildFrame();
            var controlViewItem = _masterFrame.View.ToCompositeView().GetItems<ControlViewItem>().FirstOrDefault();
            if (controlViewItem != null){
                controlViewItem.ControlCreated+=ControlViewItemOnControlCreated;
            }
            else{
                _masterFrame.View.SelectionChanged += (_, _) => _childFrame.View.SetCurrentObject(_masterFrame.View.CurrentObject);
            }
        }

        private void ControlViewItemOnControlCreated(object sender, EventArgs e){
            var userControl = (IUserControl)((ControlViewItem)sender).Control;
            _masterFrame.ActiveActions().ForEach(action => action.SelectionContext = userControl);
            userControl.CurrentObjectChanged += UserControlOnCurrentObjectChanged;
            userControl.ProcessObject+=UserControlOnProcessObject;
            _masterFrame.View.ObjectSpace.Committed += (_, _) => userControl.Refresh();
        }
        
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
    
    public interface IUserControl:ISelectionContext,IComplexControl{
        void Refresh(object currentObject);
        event EventHandler ProcessObject;
        void SetCriteria<T>(Expression<Func<T, bool>> lambda);
        void SetCriteria(string criteria);
        void SetCriteria(LambdaExpression lambda);
        Type ObjectType{ get; }
    }
}