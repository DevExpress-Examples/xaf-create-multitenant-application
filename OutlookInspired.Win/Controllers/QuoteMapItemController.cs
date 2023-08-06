using DevExpress.ExpressApp.Chart.Win;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.Controllers{
    public class QuoteMapItemController:Module.Controllers.Quote.QuoteMapItemController{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is ChartListEditor chartListEditor){
                chartListEditor.ControlDataSourceChanged += (_, _) => {
                    var chartControlSeries = chartListEditor.ChartControl.Series[0];
                    chartControlSeries.ArgumentDataMember = nameof(QuoteMapItem.Name);
                    chartControlSeries.ValueDataMembers.AddRange(nameof(QuoteMapItem.Value).YieldItem().ToArray());
                };
                
            }
        }
    }
}