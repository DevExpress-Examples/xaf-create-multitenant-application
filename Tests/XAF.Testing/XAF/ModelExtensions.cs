using System.Reflection;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;

namespace XAF.Testing.XAF{
    public static class ModelExtensions{
        public static bool IsOneToOneForeignKey(this IMemberInfo memberInfo) 
            => memberInfo.Name.Length > 2 && memberInfo.Name.EndsWith("Id") && (memberInfo.Owner
                .FindMember(memberInfo.Name.Substring(0, memberInfo.Name.Length - 2))?.IsOneToOneRelated() ?? false);

        public static bool IsOneToOneRelated(this IMemberInfo memberInfo) 
            => ((memberInfo.AssociatedMemberInfo?.IsPersistent ?? false)&&!memberInfo.AssociatedMemberInfo.IsList) || memberInfo.IsOneToOneForeignKey();

        public static bool IsDefault(this IModelObjectView modelObjectView) 
            => modelObjectView is IModelListView modelListView ? modelObjectView.ModelClass.DefaultListView == modelListView
                : modelObjectView.ModelClass.DefaultDetailView == modelObjectView;

        public static IModelMemberViewItem[] VisibleMemberViewItems(this IModelObjectView modelObjectView) => modelObjectView.MemberViewItems().VisibleMemberViewItems().ToArray();
        public static IEnumerable<IModelMemberViewItem> MemberViewItems(this IModelView modelObjectView, Type propertyEditorType=null)
            => !(modelObjectView is IModelObjectView) ? Enumerable.Empty<IModelMemberViewItem>()
                : (modelObjectView is IModelListView modelListView ? modelListView.Columns : ((IModelDetailView) modelObjectView).Items.OfType<IModelMemberViewItem>())
                .Where(item => propertyEditorType == null || propertyEditorType.IsAssignableFrom(item.PropertyEditorType));
        public static IModelMemberViewItem[] VisibleMemberViewItems(this IEnumerable<IModelMemberViewItem> modelMemberViewItems) => modelMemberViewItems
            .Where(item => !item.Index.HasValue || item.Index > -1).ToArray();

        public static IEnumerable<IModelViewLayoutElement> Flatten(this IModelViewLayout viewLayout) 
            => viewLayout.OfType<IModelLayoutGroup>()
                .SelectMany(group => group.SelectManyRecursive(element => element as IEnumerable<IModelViewLayoutElement>));
    }
}