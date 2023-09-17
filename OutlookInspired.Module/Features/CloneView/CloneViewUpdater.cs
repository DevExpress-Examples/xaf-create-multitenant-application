using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Features.CloneView;
public class CloneViewUpdater : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator> {
    public override void UpdateNode(ModelNode node) 
        => node.Application.BOModel.SelectMany(modelClass => modelClass.Attributes<CloneViewAttribute>()
                .OrderBy(viewAttribute => viewAttribute.ViewType)
                .Do(attribute => GetModelView(modelClass,attribute.ViewType).CreateView( attribute.ViewId,attribute.DetailView)))
            .Enumerate();
    IModelView GetModelView(IModelClass modelClass, CloneViewType viewType) 
        => viewType == CloneViewType.LookupListView ? modelClass.DefaultLookupListView
            : viewType == CloneViewType.DetailView ? modelClass.DefaultDetailView : modelClass.DefaultListView;
}