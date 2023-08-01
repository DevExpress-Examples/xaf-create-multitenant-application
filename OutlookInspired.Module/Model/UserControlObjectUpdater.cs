using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Model{
    public class UserControlObjectUpdater:ModelNodesGeneratorUpdater<ModelViewsNodesGenerator>{
        public override void UpdateNode(ModelNode node) 
            => ((IModelViews)node).OfType<IModelDetailView>().Where(view => view.ModelClass.TypeInfo.Type==typeof(UserControlObject))
                .Do(modelDetailView => {
                    modelDetailView.AllowEdit = false;
                    modelDetailView.AllowDelete = false;
                    modelDetailView.AllowNew = false;
                })
                .Cast<IModelViewHiddenActions>()
                .SelectMany(actions => new []{"Save","Refresh","SaveAndClose","SaveAndNew","ShowAllContexts"}.Do(actionId => actions.HiddenActions.AddNode<IModelActionLink>(actionId)))
            .Enumerate();
    }
}