using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraMap;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Controllers;
using OutlookInspired.Module.Services;
using OutlookInspired.Win.Extensions;
using MapItem = OutlookInspired.Module.BusinessObjects.MapItem;

namespace OutlookInspired.Win.Controllers.Maps{
    public class SalesMapsViewController:WinMapsViewController<ISalesMapsMarker>{
        private VectorItemsLayer _itemsLayer;
        private PieChartDataAdapter _pieChartDataAdapter;

        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (Frame is NestedFrame)return;
            _itemsLayer.DataLoaded-=ItemsLayerOnDataLoaded;
            Frame.GetController<MapsViewController>().SalesPeriodAction.Executed-=SalesPeriodActionOnExecuted;
        }

        protected override void OnActivated(){
            base.OnActivated();
            if (Frame is NestedFrame)return;
            Frame.GetController<MapsViewController>().SalesPeriodAction.Executed+=SalesPeriodActionOnExecuted;
        }

        private void SalesPeriodActionOnExecuted(object sender, ActionBaseEventArgs e){
            SetPieAdapterDataSource();
        }

        protected override void CustomizeMapControl(){
            MapControl.Layers.AddRange(new LayerBase[]{
                new ImageLayer{ DataProvider = MapDataProvider },
                _itemsLayer=new VectorItemsLayer(){
                    Colorizer =new Colorizer{ItemKeyProvider = new ArgumentItemKeyProvider()},
                    ToolTipPattern = $"{nameof(MapItem.City)}:%A% {nameof(MapItem.Total)}:%V%",
                    Data = _pieChartDataAdapter=new PieChartDataAdapter(){
                        Mappings ={
                            Latitude =nameof(MapItem.Latitude),Longitude =nameof(MapItem.Longitude),
                            PieSegment = nameof(MapItem.ProductName), Value = nameof(MapItem.Total) },
                        PieItemDataMember = nameof(MapItem.City) 
                    }
                }
            });
            SetPieAdapterDataSource();
            _itemsLayer.DataLoaded+=ItemsLayerOnDataLoaded;
        }

        private void SetPieAdapterDataSource() 
            => _pieChartDataAdapter.DataSource = ((ISalesMapsMarker)View.CurrentObject)
                .SaleMapItems((Period)Frame.GetController<MapsViewController>().SalesPeriodAction.SelectedItem.Data);

        private void ItemsLayerOnDataLoaded(object sender, DataLoadedEventArgs e){
            var mapItem = _itemsLayer.Data.Items.FirstOrDefault();
            _itemsLayer.SelectedItem = mapItem != null ? _itemsLayer.Data.GetItemSourceObject(mapItem) : null;
            ZoomToRegionService.ZoomTo(((ISalesMapsMarker)View.CurrentObject)
                .Stores((Period)Frame.GetController<MapsViewController>().SalesPeriodAction.SelectedItem.Data).ToArray());
        }
    }
}