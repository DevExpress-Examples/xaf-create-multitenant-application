using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.ViewVariantsModule;

namespace OutlookInspired.Module.Features{
    public class NavigationItemsModelUpdater:ModelNodesGeneratorUpdater<NavigationItemNodeGenerator>{
        public override void UpdateNode(ModelNode node){
            IModelNavigationItemsVariantSettings itemsVariantSettings = (IModelNavigationItemsVariantSettings) node;
            if (itemsVariantSettings == null)
                return;
        }
    }
}