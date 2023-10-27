using OutlookInspired.Blazor.Server.Components.DevExtreme.Charts;
using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Features.Quotes{
    public class DxFunnelModel:RootListViewComponentModel<QuoteMapItem,Components.DevExtreme.Charts.DxFunnelModel,DxFunnel> {
        public override void Refresh(){
            base.Refresh();
            var dataSource = Objects.Select(item => new{item.Value,item.Name}).Cast<object>().ToArray();
            ComponentModel.Options.DataSource = dataSource;
            ComponentModel.Options.PaletteData = dataSource.Length.DistinctColors().Select((color, i) => ( color,stage:(Stage)i)).ToArray();
            ComponentModel.Update?.Invoke();
        }

        public override void Refresh(object currentObject){ }

        public override Components.DevExtreme.Charts.DxFunnelModel ComponentModel{ get; } = new(){
            Options ={
                ValueField = nameof(QuoteMapItem.Value).FirstCharacterToLower(),
                ArgumentField = nameof(QuoteMapItem.Name).FirstCharacterToLower(),
                Item ={ Border ={ Visible = false } }, Label ={ Position = "inside", Visible = true },
                Height = "90vh"
            }
        };
    }
}