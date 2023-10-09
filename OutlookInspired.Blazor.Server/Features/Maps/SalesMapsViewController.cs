using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using OutlookInspired.Blazor.Server.Components.DevExtreme.Maps;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public abstract class SalesMapsViewController<T> : BlazorMapsViewController<T,DxVectorMapModel,DxVectorMap> where T:ISalesMapsMarker {
        private MapItemChartListEditor _chartListEditor;
        private MapItem[] _mapItems;

        protected override void OnDeactivated(){
            base.OnDeactivated();
            if (!Active)return;
            MapsViewController.SalesPeriodAction.Executed-=SalesPeriodActionOnExecuted;
            Model.MapItemSelected-=ModelOnMapItemSelected;
        }

        protected override void OnActivated(){
            base.OnActivated();
            if(!Active)return;
            MapsViewController.SalesPeriodAction.Executed+=SalesPeriodActionOnExecuted;
            var salesListPropertyEditor = View.GetItems<ListPropertyEditor>().FirstOrDefault(editor => editor.MemberInfo.Name==nameof(ISalesMapsMarker.CitySales));
            if (salesListPropertyEditor != null) salesListPropertyEditor.ControlCreated += OnListPropertyEditorControlCreated;
        }
        
        private void OnListPropertyEditorControlCreated(object sender, EventArgs e){
            ((ListPropertyEditor)sender).ControlCreated-=OnListPropertyEditorControlCreated;
            _chartListEditor = ((MapItemChartListEditor)(((ListPropertyEditor)sender)!).ListView.Editor);
            _chartListEditor.ControlsCreated += ChartListEditorOnControlsCreated;
        }

        private void ChartListEditorOnControlsCreated(object sender, EventArgs e){
            _chartListEditor.ControlsCreated-=ChartListEditorOnControlsCreated;
            _chartListEditor.Control.ArgumentField = item => item.PropertyValue(View.ObjectTypeInfo.Type);
            _chartListEditor.Control.NameField = item => item.PropertyValue(View.ObjectTypeInfo.Type);
            _chartListEditor.Control.ValueField = item => item.Total;
            _chartListEditor.DataSource = _mapItems;
        }

        protected override DxVectorMapModel CustomizeModel(DxVectorMapModel model){
            _mapItems = ((ISalesMapsMarker)View.CurrentObject).Sales((Period)MapsViewController.SalesPeriodAction.SelectedItem.Data).ToArray();
            model.Options = _mapItems.VectorMapOptions<MapItem,PieLayer>(_mapItems.Palette(View.ObjectTypeInfo.Type),
                items => items.Select(item => item.Total).ToList());
            _mapItems = _mapItems.Colorize(model.Options.Layers.OfType<PieLayer>().First().Palette, View.ObjectTypeInfo.Type);
            model.MapItemSelected-=ModelOnMapItemSelected;
            model.MapItemSelected+=ModelOnMapItemSelected;
            return model;
        }

        private void ModelOnMapItemSelected(object sender, MapItemSelectedArgs e) 
            => _chartListEditor.DataSource = ((ISalesMapsMarker)View.CurrentObject)
                .Sales((Period)MapsViewController.SalesPeriodAction.SelectedItem.Data, e.Item.GetProperty(nameof(MapItem.City).FirstCharacterToLower()).GetString())
                .Colorize(Model.Options.Layers.OfType<PieLayer>().First().Palette,View.ObjectTypeInfo.Type).ToArray();

        private void SalesPeriodActionOnExecuted(object sender, ActionBaseEventArgs e){
            var model = CustomizeModel();
            model.LayerDatasource = model.Options.Layers.OfType<PieLayer>().First();
            _chartListEditor.DataSource = _mapItems;
        }
    }
}