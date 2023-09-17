using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.Controllers.Quotes{
    [Obsolete]
    public class MapItemController:ObjectViewController<ListView,QuoteMapItem>{
        protected override void OnActivated(){
            base.OnActivated();
        }

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            // if (View.Editor is DevExpress.ExpressApp.Chart.Win.ChartListEditor chartListEditor){
            //     chartListEditor.ControlDataSourceChanged += (_, _) => {
            //         var chartControlSeries = chartListEditor.ChartControl.Series[0];
            //         chartControlSeries.ArgumentDataMember = nameof(QuoteMapItem.Name);
            //         chartControlSeries.ValueDataMembers.AddRange(nameof(QuoteMapItem.Value).YieldItem().ToArray());
            //     };
            //     
            // }
        }
    }
}