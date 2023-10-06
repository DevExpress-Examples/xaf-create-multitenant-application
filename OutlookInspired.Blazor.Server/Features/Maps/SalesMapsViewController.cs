using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using OutlookInspired.Blazor.Server.Components.DevExtreme.Maps;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public abstract class SalesMapsViewController<T> : BlazorMapsViewController<T> where T:ISalesMapsMarker{
        private MapItemChartListEditor _chartListEditor;

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
            var salesListPropertyEditor = View.GetItems<ListPropertyEditor>().FirstOrDefault(editor => editor.MemberInfo.Name==nameof(Product.CitySales));
            if (salesListPropertyEditor != null) salesListPropertyEditor.ControlCreated += OnListPropertyEditorControlCreated;
        }

        
        private void OnListPropertyEditorControlCreated(object sender, EventArgs e){
            ((ListPropertyEditor)sender).HideToolBar().ControlCreated-=OnListPropertyEditorControlCreated;
            _chartListEditor = ((MapItemChartListEditor)(((ListPropertyEditor)sender)!).ListView.Editor);
            _chartListEditor.ControlsCreated += ChartListEditorOnControlsCreated;
        }

        private void ChartListEditorOnControlsCreated(object sender, EventArgs e){
            _chartListEditor.ControlsCreated-=ChartListEditorOnControlsCreated;
            _chartListEditor.Control.ArgumentField = item => item.CustomerName;
            _chartListEditor.Control.NameField = item => item.CustomerName;
            _chartListEditor.Control.ValueField = item => item.Total;
            _chartListEditor.DataSource = Model.MapSettings.MapItems;
        }

        protected override Model CustomizeModel(Model model){
            model.MapSettings = ((ISalesMapsMarker)View.CurrentObject).MapSettings((Period)MapsViewController.SalesPeriodAction.SelectedItem.Data);
            model.MapItemSelected-=ModelOnMapItemSelected;
            model.MapItemSelected+=ModelOnMapItemSelected;
            return model;
        }

        private void ModelOnMapItemSelected(object sender, MapItemSelectedArgs e) 
            => _chartListEditor.DataSource = ((ISalesMapsMarker)View.CurrentObject)
                .Sales((Period)MapsViewController.SalesPeriodAction.SelectedItem.Data, e.Item.City)
                .Colorize(Model.MapSettings.Palette).ToArray();

        private void SalesPeriodActionOnExecuted(object sender, ActionBaseEventArgs e){
            var model = CustomizeModel();
            model.ChangePeriod = true;
            _chartListEditor.DataSource = model.MapSettings.MapItems;
        }
    }
}