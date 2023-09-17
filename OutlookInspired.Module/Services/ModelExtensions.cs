using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using OutlookInspired.Module.Attributes;

namespace OutlookInspired.Module.Services{
    internal static class ModelExtensions{
        public static void CreateView(this IModelView source,  string viewId,string detailViewId=null) {
            var cloneNodeFrom = ((ModelNode)source).Clone(viewId);
            if (source is not IModelListView || string.IsNullOrEmpty(detailViewId)) return;
            ((IModelListView)cloneNodeFrom).DetailView = source.Application.Views.OfType<IModelDetailView>()
                .FirstOrDefault(view => view.Id == detailViewId)??throw new NullReferenceException(detailViewId);
        }

        public static IEnumerable<T> Attributes<T>(this IModelClass modelClass) where T:Attribute 
            => modelClass.TypeInfo.FindAttributes<T>();
        
        
        public static void Add(this ModelNodesGeneratorUpdaters updaters, params IModelNodesGeneratorUpdater[] nodeUpdaters)
            => nodeUpdaters.Do(updaters.Add).Enumerate();
        
        public static PropertyEditor NewPropertyEditor(this IModelMemberViewItem modelMemberViewItem) 
            => modelMemberViewItem.PropertyEditorType.HasPublicParameterlessConstructor()
                ? (PropertyEditor)modelMemberViewItem.PropertyEditorType.CreateInstance()
                : (PropertyEditor)Activator.CreateInstance(modelMemberViewItem.PropertyEditorType,
                    args: new object[]{ modelMemberViewItem.GetParent<IModelObjectView>().ModelClass.TypeInfo.Type, modelMemberViewItem });

        public static TNode GetParent<TNode>(this IModelNode modelNode) where TNode : class 
            => modelNode.Parent as TNode ?? modelNode.Parent?.GetParent<TNode>();

        public static IEnumerable<IModelViewLayoutElement> Flatten(this IModelViewLayout viewLayout) 
            => viewLayout.OfType<IModelLayoutGroup>()
                .SelectMany(group => group.SelectManyRecursive(element => element as IEnumerable<IModelViewLayoutElement>));

        public static IModelLayoutViewItem LayoutItem(this IModelViewItem modelViewItem)
            => modelViewItem.GetParent<IModelDetailView>().Layout.Flatten()
                .OfType<IModelLayoutViewItem>().FirstOrDefault(element => element.ViewItem.Id == modelViewItem.Id);
    }
}