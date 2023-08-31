using DevExpress.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Layout;
using DevExpress.Map.Dashboard;
using DevExpress.XtraMap;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.Controllers.Maps{
    public abstract class WinMapsViewController<T>:ObjectViewController<DetailView,T>{
        protected readonly BingMapDataProvider MapDataProvider=new(){ BingKey = MapsViewController.Key,Kind = BingMapKind.Road};
        protected MapControl MapControl;
        protected IZoomToRegionService ZoomToRegionService;

        static WinMapsViewController(){
            var _ = typeof(MapControl);
        }

        protected override void OnDeactivated(){
            base.OnActivated();
            if (Frame is NestedFrame)return;
            var mapsViewController = Frame.GetController<MapsViewController>();
            mapsViewController.ExportMapAction.Executed-=ExportMapActionOnExecuted;
            mapsViewController.PrintAction.Executed-=PrintActionOnExecuted;
            mapsViewController.PrintPreviewMapAction.Executed-=PrintPreviewMapActionOnExecuted;
        }

        protected override void OnActivated(){
            base.OnActivated();
            if (Frame is  NestedFrame)return;
            var mapsViewController = Frame.GetController<MapsViewController>();
            mapsViewController.ExportMapAction.Executed+=ExportMapActionOnExecuted;
            mapsViewController.PrintAction.Executed+=PrintActionOnExecuted;
            mapsViewController.PrintPreviewMapAction.Executed+=PrintPreviewMapActionOnExecuted;
            View.CustomizeViewItemControl<ControlViewItem>(this, item => {
                MapControl = (MapControl)item.Control;
                MapControl.ZoomLevel = 8;
                ZoomToRegionService = (IZoomToRegionService)((IServiceProvider)MapControl).GetService(typeof(IZoomToRegionService));
                CustomizeMapControl();
            });
        }

        protected abstract void CustomizeMapControl();

        private void PrintActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => MapControl.Print();

        private void PrintPreviewMapActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => MapControl.ShowRibbonPrintPreview();

        private void ExportMapActionOnExecuted(object sender, ActionBaseEventArgs e){
            using var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG files (*.png)|*.png";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = View.DefaultMemberValue().ToString();
            if (saveFileDialog.ShowDialog() == DialogResult.OK){
                MapControl.ExportToImage(saveFileDialog.FileName,DXImageFormat.Png);
            }
        }

        
    }
}