using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Win.Features.Evaluations{

    public class RemoveMenuItemController:ObjectViewController<ListView,Evaluation>{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is DevExpress.ExpressApp.Scheduler.Win.SchedulerListEditor schedulerListEditor){
                schedulerListEditor.SchedulerControl.PopupMenuShowing += (_, e) => {
                    // e.Menu.RemoveMenuItem(SchedulerMenuItemId.SwitchViewMenu);
                    // e.Menu.RemoveMenuItem(SchedulerMenuItemId.NewAllDayEvent);
                };
            }
            
        }
    }
}