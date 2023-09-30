using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Editors {
    [PropertyEditor(typeof(object), false)]
    public class MarkupContentPropertyEditor : ComponentPropertyEditor {
        public MarkupContentPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }

        protected override RenderFragment CreateViewComponentCore(object dataContext) 
            => builder => builder.RenderMarkup(MemberInfo.Name,MemberInfo.GetValue(dataContext));

        protected override void RenderComponent(RenderTreeBuilder builder) 
            => builder.RenderMarkup(MemberInfo.Name, PropertyValue);

    }

}