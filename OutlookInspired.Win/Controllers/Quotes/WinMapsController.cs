using DevExpress.ExpressApp.Actions;
using DevExpress.XtraCharts;
using DevExpress.XtraMap;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;
using OutlookInspired.Module.Services.Internal;
using OutlookInspired.Win.Controllers.Maps;
using OutlookInspired.Win.Extensions;
using OutlookInspired.Win.Extensions.Internal;
using ElementSelectionMode = DevExpress.XtraMap.ElementSelectionMode;
using KeyColorColorize = DevExpress.XtraMap.KeyColorColorizer;

namespace OutlookInspired.Win.Controllers.Quotes{
    public class WinMapsController:WinMapsViewController<Quote>{
        private BubbleChartDataAdapter _chartDataAdapter;
        private MapCallout _callOut;
        private VectorItemsLayer _itemsLayer;

        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (!Active)return;
            MapControl.SelectionChanged-=MapControlOnSelectionChanged;
            MapsViewController.StageAction.Executed-=StageActionOnExecuted;
        }

        protected override void OnActivated(){
            base.OnActivated();
            if (!Active)return;
            MapsViewController.StageAction.Executed+=StageActionOnExecuted;
        }
        
        private void StageActionOnExecuted(object sender, ActionBaseEventArgs e) => SetAdapterDataSource();

        protected override void CustomizeMapControl(){
            MapControl.SelectionMode=ElementSelectionMode.Single;
            _callOut = new MapCallout{ AllowHtmlText = true };
            _itemsLayer = CreateItemsLayer();
            MapControl.Layers.AddRange(new LayerBase[]{ _itemsLayer,
                new VectorItemsLayer{Data =new MapItemStorage(){Items = {  _callOut}} }
            });
            MapControl.SelectionChanged+=MapControlOnSelectionChanged;
            SetAdapterDataSource();
        }

        private VectorItemsLayer CreateItemsLayer(){
            var itemsLayer = new VectorItemsLayer(){
                Colorizer = new KeyColorColorize{ ItemKeyProvider = new ArgumentItemKeyProvider() },
                ToolTipPattern = $"{nameof(QuoteMapItem.City)}:%A% {nameof(QuoteMapItem.Value)}:%V%",
                Data = _chartDataAdapter = new BubbleChartDataAdapter(){
                    Mappings ={
                        Latitude = nameof(QuoteMapItem.Latitude), Longitude = nameof(QuoteMapItem.Longitude),
                        Value = nameof(QuoteMapItem.Value)
                    },
                    BubbleItemDataMember = nameof(QuoteMapItem.City), BubbleGroupMember = nameof(QuoteMapItem.Index)
                }
            };
            itemsLayer.DataLoaded += ItemsLayerOnDataLoaded;
            return itemsLayer;
        }
        
        Stage Stage => (Stage)MapsViewController.StageAction.SelectedItem.Data;
        public PaletteEntry[] PaletteEntries{ get; set; }

        private void SetAdapterDataSource(){
            _itemsLayer.ItemStyle.Fill = PaletteEntries[Array.IndexOf(Enum.GetValues(typeof(Stage)), Stage)].Color;
            _chartDataAdapter.DataSource = ObjectSpace.Opportunities(Stage);
        }

        private void ItemsLayerOnDataLoaded(object sender, DataLoadedEventArgs e){
            var itemsLayer = (VectorItemsLayer)sender;
            var mapItem = itemsLayer.Data.Items.FirstOrDefault();
            itemsLayer.SelectedItem = (mapItem != null) ? itemsLayer.Data.GetItemSourceObject(mapItem) : null;
            Zoom.To(ObjectSpace.Stores(Stage));
        }

        private void MapControlOnSelectionChanged(object sender, MapSelectionChangedEventArgs e){
            if (e.Selection.FirstOrDefault() is not QuoteMapItem mapItem) return;
            _callOut.Location = mapItem.ToGeoPoint();
            using var objectSpace = Application.NewObjectSpace();
            var opportunity = objectSpace.Opportunity(Stage, mapItem.City);
            _callOut.Text = $"TOTAL<br><color=206,113,0><b><size=+4>{opportunity:c}</color></size></b><br>{mapItem.City}";

        }
    }
}