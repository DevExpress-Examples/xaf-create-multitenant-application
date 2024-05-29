using DevExpress.Blazor.Reporting.Models;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.ComponentModels;
using EditorAliases = OutlookInspired.Module.Services.EditorAliases;

namespace OutlookInspired.Blazor.Server.Editors {
    [PropertyEditor(typeof(byte[]), EditorAliases.PdfViewerEditor)]
    public class PdfViewerPropertyEditor : BlazorPropertyEditorBase {
        public PdfViewerPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        public override PdfViewerModel ComponentModel => (PdfViewerModel)base.ComponentModel;
        protected override IComponentModel CreateComponentModel() => new PdfViewerModel {
            CssClass = "pe-pdf-viewer",
            CustomizeToolbar = EventCallback.Factory.Create<ToolbarModel>(this, (m) => { m.AllItems.Clear(); })
        };
        protected override void ReadValueCore() {
            base.ReadValueCore();
            ComponentModel.DocumentContent = (byte[])PropertyValue;
        }
    }
}