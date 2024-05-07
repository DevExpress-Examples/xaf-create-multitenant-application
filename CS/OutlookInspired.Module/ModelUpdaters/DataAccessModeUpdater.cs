using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Module.ModelUpdaters{
    
    public class DataAccessModeUpdater : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator>{
        public static readonly Type[] ClientTypes = {typeof(Evaluation),typeof(TaskAttachedFile)};

        public override void UpdateNode(ModelNode node) 
            => ((IModelViews)node).OfType<IModelListView>()
                .Where(view => !ClientTypes.Contains(view.ModelClass.TypeInfo.Type))
                .Do(view => view.DataAccessMode=view.ModelClass.TypeInfo.IsPersistent?CollectionSourceDataAccessMode.Server:CollectionSourceDataAccessMode.Client)
                .Enumerate();
    }
}