using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Chart.Win;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraMap;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services.Internal;
using OutlookInspired.Win.Extensions.Internal;
using KeyColorColorizer = DevExpress.XtraMap.KeyColorColorizer;
using MapItem = OutlookInspired.Module.BusinessObjects.MapItem;

namespace OutlookInspired.Win.Features.Maps{
    public abstract class SalesMapsViewController<T>:WinMapsViewController<T> where T:ISalesMapsMarker{
        private VectorItemsLayer _itemsLayer;
        private PieChartDataAdapter _pieChartDataAdapter;
        private ISalesMapsMarker _salesMapsMarker;

        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (!Active)return;
            _itemsLayer.DataLoaded-=ItemsLayerOnDataLoaded;
            MapsViewController.SalesPeriodAction.Executed-=SalesPeriodActionOnExecuted;
            MapControl.SelectionChanged-=MapControlOnSelectionChanged;
        }

        protected override void OnActivated(){
            base.OnActivated();
            if (!Active)return;
            MapsViewController.SalesPeriodAction.Executed+=SalesPeriodActionOnExecuted;
            _salesMapsMarker = ((ISalesMapsMarker)View.CurrentObject);
        }

        private void SalesPeriodActionOnExecuted(object sender, ActionBaseEventArgs e) 
            => SetPieAdapterDataSource();

        protected override void CustomizeMapControl(){
            MapControl.Layers.AddRange(new LayerBase[]{
                _itemsLayer=new VectorItemsLayer(){
                    Colorizer =new Colorizer{ItemKeyProvider = new ArgumentItemKeyProvider()},
                    ToolTipPattern = $"{nameof(MapItem.City)}:%A% {nameof(MapItem.Total)}:%V%",
                    Data = _pieChartDataAdapter=new PieChartDataAdapter(){
                        Mappings ={
                            Latitude =nameof(MapItem.Latitude),Longitude =nameof(MapItem.Longitude),
                            PieSegment = View.ObjectTypeInfo.Type.MapItemProperty(), Value = nameof(MapItem.Total) },
                        PieItemDataMember = nameof(MapItem.City) ,SummaryFunction = SummaryFunction.Sum,
                    }
                }
            });
            SetPieAdapterDataSource();
            _itemsLayer.DataLoaded+=ItemsLayerOnDataLoaded;
            MapControl.SelectionChanged+=MapControlOnSelectionChanged;
        }

        private void MapControlOnSelectionChanged(object sender, MapSelectionChangedEventArgs e){
            var chartListEditor = (ChartListEditor)View.GetItems<ListPropertyEditor>()
                .First(editor => editor.ListView?.Editor is ChartListEditor)
                .HideToolBar().ListView.Editor;
            chartListEditor.DataSource = ((MapItem[])_pieChartDataAdapter.DataSource).Where(item => item.City==((MapItem)_itemsLayer.SelectedItem)?.City).ToArray();
            chartListEditor.ChartControl.ApplyColors((KeyColorColorizer)_itemsLayer.Colorizer);
        }

        private Period Period => (Period)MapsViewController.SalesPeriodAction.SelectedItem.Data;
        private void SetPieAdapterDataSource() 
            => _pieChartDataAdapter.DataSource = _salesMapsMarker.Sales(Period);

        private void ItemsLayerOnDataLoaded(object sender, DataLoadedEventArgs e){
            var mapItem = _itemsLayer.Data.Items.FirstOrDefault();
            _itemsLayer.SelectedItem = mapItem != null ? _itemsLayer.Data.GetItemSourceObject(mapItem) : null;
            Zoom.To(_salesMapsMarker.Stores(Period).ToArray());
        }
    }
}