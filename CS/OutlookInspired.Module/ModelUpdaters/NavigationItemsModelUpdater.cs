using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.SystemModule;
using OutlookInspired.Module.Services.Internal;
using static OutlookInspired.Module.ModelUpdaters.DashboardViewsModelUpdater;

namespace OutlookInspired.Module.ModelUpdaters{
    public class NavigationItemsModelUpdater:ModelNodesGeneratorUpdater<NavigationItemNodeGenerator>{
        public const string EvaluationListView = "Evaluation_ListView";
        public const string ModelDifferenceListView = "ModelDifference_ListView";
        public const string WelcomeDetailView = "Welcome_DetailView";
        public const string ReportDataV2ListView = "ReportDataV2_ListView";
        public const string UserListView = "ApplicationUser_ListView";
        public const string RoleListView = "PermissionPolicyRole_ListView";
        public const string RichTextMailMergeDataListView = "RichTextMailMergeData_ListView";
        
        public override void UpdateNode(ModelNode node){
            node.Nodes.SelectMany(modelNode => modelNode.Nodes).Cast<IModelNode>().ToArray().Do(modelNode => modelNode.Remove()).Enumerate();
            DashboardViews.Concat(WelcomeDetailView, EvaluationListView)
                .Do(view => node.Application.NewNavigationItem("Default",view)).Enumerate();
            new[]{UserListView,RoleListView,ModelDifferenceListView}
                .Do(view => node.Application.NewNavigationItem("Admin Portal",view)).Enumerate();
            new []{ReportDataV2ListView,RichTextMailMergeDataListView}
                .Do(view => node.Application.NewNavigationItem("Reports",view)).Enumerate();
            ((IModelNavigationItem)node.Nodes.First()["Reports"]).ImageName = "Navigation_Item_Report";
        }
    }
}