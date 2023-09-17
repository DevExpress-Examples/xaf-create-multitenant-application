using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraCharts;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Module.Features;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.Controllers.Quotes{
    public class PaletteEntriesController:ViewController<DashboardView>{
        private PaletteEntry[] _paletteEntries;
        public PaletteEntriesController() => TargetViewId = "Opportunities";
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            ((DevExpress.ExpressApp.Chart.Win.ChartListEditor)View.ChildFrame().View.ToListView().Editor)
                .ControlsCreated+=ChartListEditorOnControlsCreated;
            View.MasterFrame().GetController<MapsViewController>().MapItAction.Executed+=MapItActionOnExecuted;
        }

        private void ChartListEditorOnControlsCreated(object sender, EventArgs e) 
            => _paletteEntries = ((DevExpress.ExpressApp.Chart.Win.ChartListEditor)sender).ChartControl
                .GetPaletteEntries(Enum.GetValues(typeof(Stage)).Length);

        protected override void OnDeactivated(){
            base.OnDeactivated();
            View.MasterFrame().GetController<MapsViewController>().MapItAction.Executed-=MapItActionOnExecuted;
        }

        private void MapItActionOnExecuted(object sender, ActionBaseEventArgs e){
            var opportunitiesWinMapsController = Application.CreateController<WinMapsController>();
            opportunitiesWinMapsController.PaletteEntries = _paletteEntries;
            e.ShowViewParameters.Controllers.Add(opportunitiesWinMapsController);
        }
    }
}