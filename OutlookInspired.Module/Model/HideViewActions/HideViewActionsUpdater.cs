using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Model.HideViewActions{
    public abstract class HideViewActionsUpdater:ModelNodesGeneratorUpdater<ModelViewsNodesGenerator>{
        public override void UpdateNode(ModelNode node)
            => ViewIds()
                .Select(id => node[id]).Cast<IModelDetailView>().Cast<IModelViewHiddenActions>()
                .SelectMany(actions => ActionIds()
                    .Do(actionId => actions.HiddenActions.AddNode<IModelActionLink>(actionId)))
                .Enumerate();

        protected abstract string[] ActionIds();
        protected abstract string[] ViewIds();


    }
}