using OutlookInspired.Blazor.Server.Components.DevExtreme;
using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Features.Quotes{
    public class DxFunnelModel:RootListViewComponentModel<QuoteMapItem,Components.DevExtreme.DxFunnelModel,DxFunnel> {
        public override void Refresh(){
            base.Refresh();
            ComponentModel.Options.DataSource = Objects.Select(item => new{item.Value,item.Name}).Cast<object>().ToArray();
        }

        protected override Components.DevExtreme.DxFunnelModel ComponentModel{ get; } = new(){
            Options ={
                ValueField = nameof(QuoteMapItem.Value).FirstCharacterToLower(),
                ArgumentField = nameof(QuoteMapItem.Name).FirstCharacterToLower(),
                Item ={ Border ={ Visible = false } }, Label ={ Position = "inside", Visible = true },
                Height = "90vh"
            }
        };
    }
}