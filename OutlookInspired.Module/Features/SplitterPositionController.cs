using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;

namespace OutlookInspired.Module.Features{
    public interface IModelListViewSplitterRelativePosition{
        [Category(OutlookInspiredModule.ModelCategory)]
        int RelativePosition{ get; set; }
    }
    public class SplitterPositionController:ViewController<ListView>,IModelExtender{
        protected override void OnActivated(){
            base.OnActivated();
            Active[nameof(SplitterPositionController)] =
                View.Model.MasterDetailMode == MasterDetailMode.ListViewAndDetailView &&
                ((IModelListViewSplitterRelativePosition)View.Model.SplitLayout).RelativePosition > 0;
        }
        
        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders) 
            => extenders.Add<IModelListViewSplitLayout,IModelListViewSplitterRelativePosition>();

    }
}