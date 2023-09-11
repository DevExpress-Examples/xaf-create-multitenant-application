using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;

namespace OutlookInspired.Module.Controllers{
    public interface IModelListViewSplitterRelativePosition{
        [Category(OutlookInspiredModule.ModelCategory)]
        int RelativePosition{ get; set; }
    }
    public class SplitterPositionRelativeSizeController:ViewController<ListView>,IModelExtender{
        protected override void OnActivated(){
            base.OnActivated();
            Active[nameof(SplitterPositionRelativeSizeController)] =
                View.Model.MasterDetailMode == MasterDetailMode.ListViewAndDetailView &&
                ((IModelListViewSplitterRelativePosition)View.Model.SplitLayout).RelativePosition > 0;
        }
        
        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) 
            => extenders.Add<IModelListViewSplitLayout,IModelListViewSplitterRelativePosition>();

    }
}