using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Utils;

namespace OutlookInspired.Module.Services.Internal{
    internal static class ModelExtensions{
        public static IEnumerable<IModelMemberViewItem> MemberViewItems(this IModelView modelObjectView, Type propertyEditorType=null)
            => !(modelObjectView is IModelObjectView) ? Enumerable.Empty<IModelMemberViewItem>()
                : (modelObjectView is IModelListView modelListView ? modelListView.Columns : ((IModelDetailView) modelObjectView).Items.OfType<IModelMemberViewItem>())
                .Where(item => propertyEditorType == null || propertyEditorType.IsAssignableFrom(item.PropertyEditorType));

        public static IModelMemberViewItem[] VisibleMemberViewItems(this IModelObjectView modelObjectView) 
            => modelObjectView.MemberViewItems().VisibleMemberViewItems().ToArray();

        public static IModelMemberViewItem[] VisibleMemberViewItems(this IEnumerable<IModelMemberViewItem> modelMemberViewItems) 
            => modelMemberViewItems.Where(item => item.Index is null or > -1).ToArray();
        public static void CreateView(this IModelView source,  string viewId,string detailViewId=null) {
            var cloneNodeFrom = ((ModelNode)source).Clone(viewId);
            if (source is not IModelListView || string.IsNullOrEmpty(detailViewId)) return;
            ((IModelListView)cloneNodeFrom).DetailView = source.Application.Views.OfType<IModelDetailView>()
                .FirstOrDefault(view => view.Id == detailViewId)??throw new NullReferenceException(detailViewId);
        }

        public static byte[] ImageBytes(this IModelApplication modelApplication,Type objectType) 
            => ImageLoader.Instance.GetImageInfo(modelApplication.BOModel.GetClass(objectType).ImageName).ImageBytes;

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