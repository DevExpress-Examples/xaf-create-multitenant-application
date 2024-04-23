using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using OutlookInspired.Blazor.Server.Components;
using EditorAliases = OutlookInspired.Module.Services.EditorAliases;

namespace OutlookInspired.Blazor.Server.Editors{
    [PropertyEditor(typeof(byte[]),EditorAliases.PdfViewerEditor)]
    public class PdfViewerEditor:BlazorPropertyEditor<PdfViewer,PdfModel>{
        public PdfViewerEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }

        protected override void ReadValueCore(){
            base.ReadValueCore();
            ComponentModel.Bytes = (byte[])PropertyValue;
        }
    }
}