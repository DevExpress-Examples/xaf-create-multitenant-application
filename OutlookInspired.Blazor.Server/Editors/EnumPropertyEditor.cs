using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Editors{
    [PropertyEditor(typeof(Enum),false)]
    public class EnumPropertyEditor:DevExpress.ExpressApp.Blazor.Editors.EnumPropertyEditor{
        public EnumPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }
        
        protected override RenderFragment CreateViewComponentCore(object dataContext) 
            => ComboBoxIconItem.Create(null, ((Enum)this.GetPropertyValue(dataContext)).ImageName());
    }
}