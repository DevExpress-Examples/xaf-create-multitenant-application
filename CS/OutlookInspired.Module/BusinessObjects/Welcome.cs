using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.Attributes.Appearance;
using BaseEditorAliases = DevExpress.ExpressApp.Editors.EditorAliases;

namespace OutlookInspired.Module.BusinessObjects {
    [DomainComponent]
    [ForbidCRUD]
    [ForbidNavigation]
    [ImageName("About")]
    public class Welcome : NonPersistentBaseObject {
        public Welcome() {
            var assembly = GetType().Assembly;
            About = Bytes(assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().First(s => s.EndsWith("Welcome.pdf"))));
        }

        byte[] Bytes(Stream stream) {
            if(stream is MemoryStream memoryStream) {
                return memoryStream.ToArray();
            }

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        [EditorAlias(BaseEditorAliases.PdfViewerPropertyEditor)]
        public byte[] About { get; set; }
    }
}
