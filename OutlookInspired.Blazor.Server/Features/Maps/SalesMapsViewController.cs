using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using OutlookInspired.Blazor.Server.Components.DevExtreme;
using OutlookInspired.Blazor.Server.Editors;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public class SalesMapsViewController : BlazorMapsViewController<ISalesMapsMarker>{
        private ChartListEditor _chartListEditor;

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
            ((ListPropertyEditor)sender).ControlCreated-=OnListPropertyEditorControlCreated;
            _chartListEditor = ((ChartListEditor)(((ListPropertyEditor)sender)!).ListView.Editor);
            _chartListEditor.ControlsCreated += ChartListEditorOnControlsCreated;
        }

        private void ChartListEditorOnControlsCreated(object sender, EventArgs e){
            _chartListEditor.ControlsCreated-=ChartListEditorOnControlsCreated;
            _chartListEditor.DataSource = Model.MapSettings.MapItems;
        }

        protected override Model CustomizeModel(Model model){
            model.MapSettings = new MapSettings(){ MapItems = ((ISalesMapsMarker)View.CurrentObject)
                .Sales((Period)MapsViewController.SalesPeriodAction.SelectedItem.Data).ToArray() };
            model.MapItemSelected-=ModelOnMapItemSelected;
            model.MapItemSelected+=ModelOnMapItemSelected;
            return model;
        }

        private void ModelOnMapItemSelected(object sender, MapItemSelectedArgs e) 
            => _chartListEditor.DataSource = ((ISalesMapsMarker)View.CurrentObject)
                .Sales((Period)MapsViewController.SalesPeriodAction.SelectedItem.Data, e.Item.City).ToArray();

        private void SalesPeriodActionOnExecuted(object sender, ActionBaseEventArgs e){
            var model = CustomizeModel();
            model.ChangePeriod = true;
            _chartListEditor.DataSource = model.MapSettings.MapItems;
        }
    }
}