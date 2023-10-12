using System.Reflection;
using DevExpress.ExpressApp.Model;

namespace XAF.Testing.XAF{
    public static class ModelExtensions{
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
        public static T CreateInstance<T>(this Type type) => (T)CreateInstance(type);

        public static object CreateInstance(this Type type){
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type);
            throw new InvalidOperationException($"Type {type.FullName} does not have a parameterless constructor.");
        }

        public static object GetPropertyValue(this object obj, string propertyName) 
            => obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!.GetValue(obj);

        public static void SetPropertyValue(this object obj, string propertyName, object value) 
            => obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!
                .SetValue(obj, value);

        public static IEnumerable<IModelViewLayoutElement> Flatten(this IModelViewLayout viewLayout) 
            => viewLayout.OfType<IModelLayoutGroup>()
                .SelectMany(group => group.SelectManyRecursive(element => element as IEnumerable<IModelViewLayoutElement>));
    }
}