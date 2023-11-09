using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Module.ModelUpdaters{
    public class DashboardViewsModelUpdater : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator>{
        public const string CustomerListView = "CustomerListView";
        public const string EmployeeListView = "EmployeeListView";
        public const string EmployeeCardListView = "EmployeeCardListView";
        public const string CustomerCardListView = "CustomerCardListView";
        public const string Opportunities = "Opportunities";
        public const string OrderListView = "OrderListView";
        public const string OrderGridView = "OrderGridView";
        public const string ProductListView = "ProductListView";
        public const string ProductCardView = "ProductCardView";
        public override void UpdateNode(ModelNode node) 
            => DashboardViews.Do(view =>  ((IModelViews)node).AddNode<IModelDashboardView>(view)).Enumerate();

        public static string[] DashboardViews 
            => new[]{CustomerListView,CustomerCardListView,EmployeeListView,Opportunities,OrderListView,OrderGridView,ProductListView,ProductCardView};
    }
}