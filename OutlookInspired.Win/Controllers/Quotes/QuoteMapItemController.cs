using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Win.Controllers.Quotes{
    public class QuoteMapItemController:ObjectViewController<ListView,QuoteMapItem>{
        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is DevExpress.ExpressApp.Chart.Win.ChartListEditor chartListEditor){
                chartListEditor.ControlDataSourceChanged += (_, _) => {
                    var chartControlSeries = chartListEditor.ChartControl.Series[0];
                    chartControlSeries.ArgumentDataMember = nameof(QuoteMapItem.Name);
                    chartControlSeries.ValueDataMembers.AddRange(nameof(QuoteMapItem.Value).YieldItem().ToArray());
                };
                
            }
        }
    }
}