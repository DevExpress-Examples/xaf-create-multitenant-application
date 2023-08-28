using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace OutlookInspired.Module.Services{
    internal static class ModelExtensions{
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