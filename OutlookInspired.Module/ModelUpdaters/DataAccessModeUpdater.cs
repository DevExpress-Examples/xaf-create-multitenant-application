using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Module.ModelUpdaters{
    [Obsolete("set the Product.OrderItems listview instead of this updater")]

    public class DataAccessModeUpdater : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator>{
        public override void UpdateNode(ModelNode node) 
            => ((IModelViews)node).OfType<IModelListView>()
                .Where(view => view.ModelClass.TypeInfo.Type!=typeof(Evaluation)||view.ModelClass.DefaultListView!=view)
                // .Where(view => new []{typeof(Order),typeof(OrderItem)}.Contains(view.ModelClass.TypeInfo.Type))
                .Do(view => view.DataAccessMode=view.ModelClass.TypeInfo.IsPersistent?CollectionSourceDataAccessMode.Server:CollectionSourceDataAccessMode.Client)
                .Enumerate();
    }
}