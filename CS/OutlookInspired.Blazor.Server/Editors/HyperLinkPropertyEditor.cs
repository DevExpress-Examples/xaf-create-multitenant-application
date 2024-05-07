using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using OutlookInspired.Blazor.Server.Components;
using OutlookInspired.Module.Services.Internal;
using EditorAliases = OutlookInspired.Module.Services.EditorAliases;

namespace OutlookInspired.Blazor.Server.Editors{
    [PropertyEditor(typeof(string), EditorAliases.HyperLinkPropertyEditor, false)]
    public class HyperLinkPropertyEditor:BlazorPropertyEditor<Hyperlink,HyperlinkModel>{
        public HyperLinkPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model){
        }

        protected override void ConfigureViewComponent(HyperlinkModel model, object dataContext){
            model.Text = $"{dataContext}";
            model.Href = $"mailto:{dataContext}";
        }
    }
    
    
    public class HyperlinkPropertyEditorUpdater:ModelNodesGeneratorUpdater<ModelViewsNodesGenerator>{
        public override void UpdateNode(ModelNode node)
            => ((IModelViews)node).OfType<IModelDetailView>()
                .SelectMany(view => view.MemberViewItems().Where(item => item.PropertyEditorType == typeof(HyperLinkPropertyEditor)))
                .Do(item => item.PropertyEditorType = typeof(StringPropertyEditor))
                .Enumerate();
    }

}