using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Editors {
    [PropertyEditor(typeof(object), Module.Services.Internal.EditorAliases.LabelPropertyEditor,false)]
    public class DisplayTextPropertyEditor : ComponentPropertyEditor {

        protected override RenderFragment CreateViewComponentCore(object dataContext) 
            => builder => builder.AddMarkupContent(0, Markup($"{this.DisplayableMemberValue(dataContext,dataContext)}"));

        private string Markup(string content) 
            => $"<div style=\"white-space: nowrap;overflow: hidden;text-overflow: ellipsis;{Model.ModelMember.MemberInfo.FontSize()};\">{content}</div>";
        
        protected override void RenderComponent(RenderTreeBuilder builder) 
            => builder.AddMarkupContent(0, Markup($"{this.DisplayableMemberValue()}"));

        public DisplayTextPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }
    }
    
}