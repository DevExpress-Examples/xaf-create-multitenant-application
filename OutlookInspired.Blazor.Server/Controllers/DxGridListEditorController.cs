using DevExpress.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.SystemModule;
using Microsoft.AspNetCore.Components;

namespace OutlookInspired.Blazor.Server.Controllers{
    public class DxGridListEditorController : ViewController<ListView> {
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            
            if (View.Editor is not DxGridListEditor{ Control: IDxGridAdapter gridAdapter }) return;
            gridAdapter.GridModel.RowDoubleClick = EventCallback.Factory.Create<GridRowClickEventArgs>(gridAdapter.GridModel,
                _ => Frame.GetController<ListViewProcessCurrentObjectController>().ProcessCurrentObjectAction.DoExecute());
            gridAdapter.GridCommandColumnModel.Visible = false;
            gridAdapter.GridModel.AllowSelectRowByClick = true;
            gridAdapter.GridModel.RowClick = default;
            var oldCustomizeElement = gridAdapter.GridModel.CustomizeElement; 
            gridAdapter.GridModel.CustomizeElement = args => { 
                oldCustomizeElement.Invoke(args);
                if (args.ElementType is not GridElementType.DataCell) return;
                if(args.CssClass != null && args.CssClass.Contains("xaf-action")) {
                    args.CssClass = args.CssClass.Replace("xaf-action", "xaf-double-click");
                } else {
                    args.CssClass += " xaf-double-click";
                }
            };
        }
    }
}