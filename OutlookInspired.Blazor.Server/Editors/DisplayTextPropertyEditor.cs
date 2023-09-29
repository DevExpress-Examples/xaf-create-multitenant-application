using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Editors {
    [PropertyEditor(typeof(object), Module.Services.Internal.EditorAliases.LabelPropertyEditor,false)]
    public class DisplayTextPropertyEditor : ComponentPropertyEditor {
        private readonly string _fontSize;
        

        public DisplayTextPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
            var fontSizeDeltaAttribute = model.ModelMember.MemberInfo.FindAttribute<FontSizeDeltaAttribute>();
            if (fontSizeDeltaAttribute != null){
                _fontSize = $"font-size: {(fontSizeDeltaAttribute.Delta == 8 ? "2" : "1.4")}rem";
            }
        }
        protected override RenderFragment CreateViewComponentCore(object dataContext) 
            => builder => builder.AddMarkupContent(0, Markup($"{this.DisplayableMemberValue(dataContext,dataContext)}"));

        private string Markup(string content) 
            => $"<div style=\"white-space: nowrap;overflow: hidden;text-overflow: ellipsis;{_fontSize};\">{content}</div>";
        
        protected override void RenderComponent(RenderTreeBuilder builder) 
            => builder.AddMarkupContent(0, Markup($"{this.DisplayableMemberValue()}"));
    }
    
}