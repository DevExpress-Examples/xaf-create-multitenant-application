using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components;
using OutlookInspired.Blazor.Server.Services;
using EditorAliases = OutlookInspired.Module.Services.Internal.EditorAliases;

namespace OutlookInspired.Blazor.Server.Editors{
    [PropertyEditor(typeof(String), EditorAliases.HyperLinkPropertyEditor, false)]
    public class HyperLinkPropertyEditor:DevExpress.ExpressApp.Blazor.Editors.BlazorPropertyEditorBase{
        public HyperLinkPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }

        protected override IComponentAdapter CreateComponentAdapter() 
            => new HyperlinkModelAdapter(new DxTextBoxModel());


        protected override RenderFragment CreateViewComponentCore(object dataContext) 
            => new HyperlinkModel(){Text = $"{dataContext}",Href = $"mailto:{dataContext}"}.Create(model1 => model1.Create<Hyperlink>());
    }
    
    public class HyperlinkModelAdapter:DxTextBoxAdapter{
        public HyperlinkModelAdapter(DxTextBoxModel componentModel) : base(componentModel){
        }

    }

}