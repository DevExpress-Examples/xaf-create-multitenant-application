using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.Attributes.Appearance;

namespace OutlookInspired.Module.BusinessObjects{
    [DomainComponent][ForbidCRUD][ForbidNavigation]
    [ImageName("About")]
    public class Welcome : NonPersistentBaseObject {
        public Welcome(){
            var assembly = GetType().Assembly;
            About = Bytes(assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().First(s => s.EndsWith("Welcome.png"))));
            Oid=Guid.Parse("8B8DF685-AD96-4BE9-A08A-8DD8C2A9F4C2");
        }

        byte[] Bytes( Stream stream){
            if (stream is MemoryStream memoryStream){
                return memoryStream.ToArray();
            }

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        
        [EditorAlias(DevExpress.ExpressApp.Editors.EditorAliases.ImagePropertyEditor)]
        public byte[] About{ get; set; }
    }
}