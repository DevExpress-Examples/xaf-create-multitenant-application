using System.ComponentModel;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using OutlookInspired.Blazor.Server.Components;
using OutlookInspired.Module;
using EditorAliases = OutlookInspired.Module.Services.Internal.EditorAliases;

namespace OutlookInspired.Blazor.Server.Editors{
    [PropertyEditor(typeof(byte[]),EditorAliases.PdfViewerEditor)]
    public class PdfViewerEditor:ComponentPropertyEditor<PdfModel,PdfModelAdapter,byte[]>{
        public PdfViewerEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }
    }

    public class PdfModelAdapter:ComponentModelAdapter<PdfViewer,PdfModel,byte[]>{
        public override void SetPropertyValue(byte[] value) => Model.Bytes = value;
        
    }

}