using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Layout;
using DevExpress.Persistent.Base;
using OutlookInspired.Blazor.Server.Components.DevExtreme;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public abstract class BlazorMapsViewController<TMapsMarker>:ObjectViewController<DetailView,TMapsMarker> where TMapsMarker:IMapsMarker{
        public Module.Features.Maps.MapsViewController MapsViewController{ get; private set; }

        public DxMapModel Model => (DxMapModel)View.GetItems<ControlViewItem>().First().Control;
        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (!Active)return;
            Frame.GetController<Module.Features.Maps.MapsViewController>().PrintAction.Executed-=PrintActionOnExecuted;
        }

        protected override void OnActivated(){
            base.OnActivated();
            if (!(Active[nameof(NestedFrame)] = Frame is not NestedFrame&&View.CurrentObject!=null))return;
            MapsViewController = Frame.GetController<Module.Features.Maps.MapsViewController>();
            MapsViewController.PrintAction.Executed+=PrintActionOnExecuted;
            View.CustomizeViewItemControl<ControlViewItem>(this, item => {
                if (item.Control is not DxMapModel model) return;
                CustomizeModel(model);
            });
        }

        protected abstract DxMapModel CustomizeModel(DxMapModel model);
        
        protected DxMapModel CustomizeModel() => CustomizeModel(Model);

        private void PrintActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => ((DxMapModel)View.GetItems<ControlViewItem>().First().Control).PrintMap = true;

    }
    public abstract class BlazorMapsViewController1<TMapsMarker>:ObjectViewController<DetailView,TMapsMarker> where TMapsMarker:IMapsMarker{
        public Module.Features.Maps.MapsViewController MapsViewController{ get; private set; }

        public DxVectorMapModel Model => (DxVectorMapModel)View.GetItems<ControlViewItem>().First().Control;
        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (!Active)return;
            Frame.GetController<Module.Features.Maps.MapsViewController>().PrintAction.Executed-=PrintActionOnExecuted;
        }

        protected override void OnActivated(){
            base.OnActivated();
            if (!(Active[nameof(NestedFrame)] = Frame is not NestedFrame&&View.CurrentObject!=null))return;
            MapsViewController = Frame.GetController<Module.Features.Maps.MapsViewController>();
            MapsViewController.PrintAction.Executed+=PrintActionOnExecuted;
            View.CustomizeViewItemControl<ControlViewItem>(this, item => {
                if (item.Control is not DxVectorMapModel model) return;
                CustomizeModel(model);
            });
        }

        protected abstract DxVectorMapModel CustomizeModel(DxVectorMapModel model);
        
        protected DxVectorMapModel CustomizeModel() => CustomizeModel(Model);

        private void PrintActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => ((DxVectorMapModel)View.GetItems<ControlViewItem>().First().Control).PrintMap = true;

    }
}