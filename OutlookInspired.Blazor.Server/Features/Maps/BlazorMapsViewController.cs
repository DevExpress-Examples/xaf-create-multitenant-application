using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Layout;
using DevExpress.Persistent.Base;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public abstract class BlazorMapsViewController<TMapsMarker>:ObjectViewController<DetailView,TMapsMarker> where TMapsMarker:IMapsMarker{
        public Module.Features.Maps.MapsViewController MapsViewController{ get; private set; }

        public Model Model => (Model)View.GetItems<ControlViewItem>().First().Control;
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
                if (item.Control is not Model model) return;
                CustomizeModel(model);
            });
        }

        protected abstract Model CustomizeModel(Model model);
        
        protected Model CustomizeModel() => CustomizeModel(Model);

        private void PrintActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => ((Model)View.GetItems<ControlViewItem>().First().Control).PrintMap = true;

    }
}