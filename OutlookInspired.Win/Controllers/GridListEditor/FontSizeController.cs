using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraGrid.Views.BandedGrid;
using OutlookInspired.Win.Extensions;
using OutlookInspired.Win.Extensions.Internal;

namespace OutlookInspired.Win.Controllers.GridListEditor{
    public class FontSizeController:ListViewControllerBase{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            View.GridView<AdvBandedGridView>()?.IncreaseFontSize(View.ObjectTypeInfo);
        }
    }
}