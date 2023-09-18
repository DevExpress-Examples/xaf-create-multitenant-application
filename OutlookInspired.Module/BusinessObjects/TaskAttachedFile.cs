using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using OutlookInspired.Module.Services;
using OutlookInspired.Module.Services.Internal;
using EditorAliases = DevExpress.ExpressApp.Editors.EditorAliases;


namespace OutlookInspired.Module.BusinessObjects{
    [ImageName("AttachFile")]
    public class TaskAttachedFile :OutlookInspiredBaseObject{
        
        public virtual EmployeeTask EmployeeTask { get; set; }
        [ExpandObjectMembers(ExpandObjectMembers.Never), RuleRequiredField()]
        public virtual FileData File { get; set; }
        [Browsable(false)]
        public virtual Guid? EmployeeTaskId{ get; set; }
        
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public string Preview 
            => File != null && Path.GetExtension(File.FileName) == ".rtf" ? File.Content.GetString() : null;
    }
}