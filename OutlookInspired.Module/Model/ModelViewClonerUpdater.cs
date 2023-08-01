using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using OutlookInspired.Module.Attributes;

namespace OutlookInspired.Module.Model;
public class ModelViewClonerUpdater : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator> {
    public override void UpdateNode(ModelNode node) {
        foreach (var modelClass in ModelClasses(node)) {
            foreach (var cloneViewAttribute in CloneViewAttributes(modelClass)) {
                var modelView = GetModelView(modelClass, cloneViewAttribute);
                var cloneNodeFrom = ((ModelNode)modelView).Clone(cloneViewAttribute.ViewId);
                AssignDetailView(node, cloneNodeFrom, modelView, cloneViewAttribute);
            }
        }
    }

    IEnumerable<CloneViewAttribute> CloneViewAttributes(IModelClass modelClass) 
        => modelClass.TypeInfo.FindAttributes<CloneViewAttribute>().OrderBy(viewAttribute => viewAttribute.ViewType);

    IEnumerable<IModelClass> ModelClasses(ModelNode node) 
        => node.Application.BOModel.Where(modelClass => modelClass.TypeInfo.FindAttribute<CloneViewAttribute>() != null);

    void AssignDetailView(ModelNode node, ModelNode cloneNodeFrom, IModelView modelView, CloneViewAttribute cloneViewAttribute) {
        if (modelView is IModelListView && !string.IsNullOrEmpty(cloneViewAttribute.DetailView)) {
            ((IModelListView)cloneNodeFrom).DetailView = node.Application.Views.OfType<IModelDetailView>().FirstOrDefault(view => view.Id == cloneViewAttribute.DetailView)??throw new NullReferenceException(cloneViewAttribute.DetailView);
        }
    }

    IModelView GetModelView(IModelClass modelClass, CloneViewAttribute cloneViewAttribute) 
        => cloneViewAttribute.ViewType == CloneViewType.LookupListView ? modelClass.DefaultLookupListView
            : cloneViewAttribute.ViewType == CloneViewType.DetailView ? modelClass.DefaultDetailView : modelClass.DefaultListView;
}