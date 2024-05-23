using DevExpress.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Controllers {
    public abstract class CellDisplayTemplateController : ViewController<ListView> {
        protected override void OnActivated() {
            base.OnActivated();
            View.Model.VisibleMemberViewItems().Skip(1).Hide();
        }

        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            if(View.Editor is DxGridListEditor editor) {
                var dataColumnModel = editor.GridDataColumnModels.First(model => model.FieldName == View.Model.VisibleMemberViewItems().First().PropertyName);
                dataColumnModel.HeaderCaptionTemplate = _ => _ => { };
                dataColumnModel.CellDisplayTemplate = Fragment();
            }
        }

        protected abstract RenderFragment<GridDataColumnCellDisplayTemplateContext> Fragment();
    }
}