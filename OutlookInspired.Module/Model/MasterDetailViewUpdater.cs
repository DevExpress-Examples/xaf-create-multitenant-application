using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Model{
    public class MasterDetailViewUpdater:ModelNodesGeneratorUpdater<ModelViewsNodesGenerator>{
        public override void UpdateNode(ModelNode node) 
            => new[]{ Customer.CustomerGridViewDetailView,Customer.CustomerLayoutViewDetailView,
                    Employee.EmployeeLayoutViewDetailView, Product.ProductCardViewDetailView,
                    Order.OrderGridViewDetailView
                }
                .Select(id => node[id]).Cast<IModelDetailView>()
                .Do(modelDetailView => modelDetailView.AllowEdit = false)
                .Cast<IModelViewHiddenActions>()
                .SelectMany(actions => new[]
                        { "Save", "SaveAndClose", "SaveAndNew", "ShowAllContexts", "NextObject", "PreviousObject" }
                    .Do(actionId => actions.HiddenActions.AddNode<IModelActionLink>(actionId)))
                .Enumerate();
    }
}