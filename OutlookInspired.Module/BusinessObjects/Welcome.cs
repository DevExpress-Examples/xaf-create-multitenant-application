using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.Attributes.Appearance;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Module.BusinessObjects{
    [DomainComponent][ForbidCRUD][ForbidNavigation]
    public class Welcome{
        public Welcome(){
            About = GetType().Assembly.GetManifestResourceStream(s => s.EndsWith("Welcome.docx")).Bytes();
        }

        [EditorAlias(DevExpress.ExpressApp.Editors.EditorAliases.RichTextPropertyEditor)]
        [FieldSize(FieldSizeAttribute.Unlimited)]
        public byte[] About{ get; set; }
    }
}