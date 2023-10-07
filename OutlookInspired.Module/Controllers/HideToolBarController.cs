using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Templates;

namespace OutlookInspired.Module.Controllers{
    public interface IModelListViewHideToolbar{
        [Category(OutlookInspiredModule.ModelCategory)]
        bool HideToolBar{ get; set; }
    }

    public class HideToolBarController:ViewController<ListView>,IModelExtender{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (((IModelListViewHideToolbar)View.Model).HideToolBar){
                ((ISupportActionsToolbarVisibility)Frame.Template).SetVisible(false);    
            }
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) 
            => extenders.Add<IModelListView,IModelListViewHideToolbar>();
    }
}