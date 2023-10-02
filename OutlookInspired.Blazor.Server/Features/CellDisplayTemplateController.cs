using DevExpress.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Features{
    public abstract class CellDisplayTemplateController:ViewController<ListView>{
        protected override void OnActivated(){
            base.OnActivated();
            View.Model.VisibleMemberViewItems().Skip(1)
                .Do(item => item.Index=-1).Enumerate();
        }

        protected override void OnViewControlsCreated(){
            base.OnViewControlsCreated();
            if (View.Editor is not DxGridListEditor{ Control: IDxGridAdapter gridAdapter }) return;
            var dataColumnModel = gridAdapter.GridDataColumnModels.First(model => model.FieldName == View.Model.VisibleMemberViewItems().First().PropertyName);
            dataColumnModel.HeaderCaptionTemplate = _ => _ => { };
            dataColumnModel.CellDisplayTemplate = Fragment();
        }

        protected abstract RenderFragment<GridDataColumnCellDisplayTemplateContext> Fragment();
    }
}