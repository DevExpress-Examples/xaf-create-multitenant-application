using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Features.Quotes.Chart{
    public class Model:RootListViewComponentModel<QuoteMapItem,Model,Chart> {
        public override void Refresh(){
            base.Refresh();
            ChartModel.Options.DataSource = Objects.Select(item => new{item.Value,item.Name}).Cast<object>().ToArray();
        }

        public Components.DevExtreme.Chart.Model ChartModel{ get; } = new(){
            Options ={
                ValueField = nameof(QuoteMapItem.Value).FirstCharacterToLower(),ArgumentField = nameof(QuoteMapItem.Name).FirstCharacterToLower(),
                Item = {Border ={Visible = false} },Label ={Position = "inside",Visible = true},Height="90vh"
            }
        };
    }
}