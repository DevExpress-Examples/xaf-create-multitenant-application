using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraGrid.Views.BandedGrid;
using OutlookInspired.Win.Extensions;

namespace OutlookInspired.Win.Controllers{
    public class FontSizeListViewController:ListViewControllerBase{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            View.GridView<AdvBandedGridView>()?.IncreaseFontSize(View.ObjectTypeInfo);
        }
    }
}