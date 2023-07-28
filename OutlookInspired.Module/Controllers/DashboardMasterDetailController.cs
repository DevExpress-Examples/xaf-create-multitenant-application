using System.ComponentModel;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers{
    
    public interface IModelDashboardViewMasterDetail{
        [Category(OutlookInspiredModule.ModelCategory)]
        bool MasterDetail{ get; set; }
    }
    public class DashboardMasterDetailController:ViewController<DashboardView>,IModelExtender{
        public DashboardMasterDetailController() => TargetViewType = ViewType.DashboardView;

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (((IModelDashboardViewMasterDetail)View.Model).MasterDetail){
                var listView = View.Views<ListView>().FirstOrDefault();
                var detailViews = View.Views<DetailView>().ToArray();
                if (listView != null&&detailViews.Length==1){
                    listView.SelectionChanged+=(_, _) => 
                        detailViews.First().SetCurrentObject(listView.SelectedObjects.Cast<object>().FirstOrDefault());                        
                }
                else{
                    var userControlController = UserControlController(detailViews);
                    userControlController.View.GetItems<ControlViewItem>().First().ControlCreated+= (sender, _) => {
                        var masterDetailService= (IMasterDetailService)((ControlViewItem)sender)!.Control;
                        masterDetailService.SelectedObjectChanged += (_, e) 
                            => detailViews.First(detailView => detailView.ObjectTypeInfo.Type!=typeof(UserControlObject)).SetCurrentObject(e.Instance);
                        masterDetailService.ProcessObject += (_, _) => userControlController.ProcessUserControlSelectedObjectAction.DoExecute();    
                    };
                }
            }
        }

        private UserControlController UserControlController(DetailView[] detailViews){
            var userControlController = View.NestedFrames<DetailView>(typeof(UserControlObject)).First()
                .GetController<UserControlController>();
            SetSelectionContext(detailViews, userControlController);
            return userControlController;
        }

        private void SetSelectionContext(DetailView[] detailViews, UserControlController userControlController){
            var childDetailView = detailViews.Except(userControlController.View.YieldItem()).First();
            userControlController.Actions.ForEach(action => action.SelectionContext = childDetailView);
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) 
            => extenders.Add<IModelDashboardView,IModelDashboardViewMasterDetail>();
    }
}