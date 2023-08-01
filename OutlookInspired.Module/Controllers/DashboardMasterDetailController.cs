using System.ComponentModel;
using System.Diagnostics;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
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
                var masterDetailType = View.MasterDetailType();
                if (masterDetailType==MasterDetailType.BothViewsNative){
                    var listView = View.Views<ListView>().First();
                    void OnSelectionChanged(object sender, EventArgs _) 
                        => View.Views<DetailView>().First().SetCurrentObject((((ListView)sender)!).SelectedObjects.Cast<object>().FirstOrDefault());
                    Deactivated+=(_, _) =>listView.SelectionChanged-=OnSelectionChanged; 
                    listView.SelectionChanged+=OnSelectionChanged;
                }
                else if (masterDetailType==MasterDetailType.MasterCustom){
                    var masterFrame = View.MasterFrame();
                    masterFrame.View.ToCompositeView().GetItems<ControlViewItem>().First().ControlCreated+= (sender, _) => {
                        var childDetailView = View.NestedFrames().First(nestedFrame => nestedFrame!=masterFrame).View.ToDetailView();
                        var userControlController = masterFrame.GetController<UserControlController>().SetSelectionContext(childDetailView);
                        var masterDetailService= (IUserControl)((ControlViewItem)sender)!.Control;
                        MasterDetailServiceOnSelectedObjectChanged(childDetailView, masterDetailService);
                        MasterDetailServiceOnProcessObject(userControlController, masterDetailService);
                    };
                }
                else if (masterDetailType==MasterDetailType.BothViewsCustom){
                    var masterFrame = View.MasterFrame();
                    masterFrame.View.ToCompositeView().GetItems<ControlViewItem>().First().ControlCreated+= (sender, _) => {
                        var childDetailView = View.NestedFrames().First(nestedFrame => nestedFrame!=masterFrame).View.ToDetailView();
                        var userControlController = masterFrame.GetController<UserControlController>().SetSelectionContext(childDetailView);
                        var masterDetailService= (IUserControl)((ControlViewItem)sender)!.Control;
                        MasterDetailServiceOnSelectedObjectChanged(childDetailView, masterDetailService);
                        MasterDetailServiceOnProcessObject(userControlController, masterDetailService);
                    };

                }
            }
        }

        private void MasterDetailServiceOnProcessObject(UserControlController userControlController, IUserControl userControl){
            void OnProcessObject(object o, ObjectArgs objectArgs) => userControlController.ProcessUserControlSelectedObjectAction.DoExecute();
            userControl.ProcessObject += OnProcessObject;
            Deactivated += (_, _) => userControl.ProcessObject -= OnProcessObject;
        }

        private void MasterDetailServiceOnSelectedObjectChanged(DetailView childDetailView,IUserControl userControl){
            void OnSelectedObjectChanged(object _, ObjectArgs e) => childDetailView.SetCurrentObject(e.Instance);
            userControl.SelectedObjectChanged+=OnSelectedObjectChanged;
            Deactivated += (_, _) => userControl.SelectedObjectChanged -= OnSelectedObjectChanged;
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) => extenders.Add<IModelDashboardView, IModelDashboardViewMasterDetail>();
    }
}