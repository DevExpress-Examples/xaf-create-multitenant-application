using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Scheduler.Win;
using DevExpress.XtraScheduler;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.Controllers.SchedulerListEditor{
    public class RemoveMenuItemController:ObjectViewController<ListView,Evaluation>{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is DevExpress.ExpressApp.Scheduler.Win.SchedulerListEditor schedulerListEditor){
                schedulerListEditor.SchedulerControl.PopupMenuShowing += (_, e) => {
                    e.Menu.RemoveMenuItem(SchedulerMenuItemId.SwitchViewMenu);
                    e.Menu.RemoveMenuItem(SchedulerMenuItemId.NewAllDayEvent);
                };
            }
            
        }
    }
}