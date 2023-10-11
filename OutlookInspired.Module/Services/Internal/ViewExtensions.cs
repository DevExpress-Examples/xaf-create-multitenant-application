using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Templates;
using OutlookInspired.Module.Features.MasterDetail;

namespace OutlookInspired.Module.Services.Internal{
    internal static class ViewExtensions{
        public static void SetNonTrackedMemberValue<T,T2>(this CompositeView view, Expression<Func<T, T2>> expression,Func<T,T2> valueSelector){
            if (view.CurrentObject==null)return;
            var propertyEditor = view.GetItems<PropertyEditor>()
                .First(editor => editor.MemberInfo.Name == expression.MemberExpressionName());
            propertyEditor.MemberInfo.SetValue(view.CurrentObject,valueSelector((T)view.CurrentObject));
            propertyEditor.ReadValue();
        }

        public static object DefaultMemberValue(this View view)
            => view.ObjectTypeInfo.DefaultMember?.GetValue(view.CurrentObject);
        
        public static IUserControl UserControl(this CompositeView view) 
            => view.GetItems<ControlViewItem>().Select(item => item.Control).OfType<IUserControl>().FirstOrDefault();

        public static DashboardViewItem ChildItem(this DashboardView view){
            var masterItem = view.MasterItem();
            return view.Items.OfType<DashboardViewItem>().First(item => item!=masterItem);
        }

        public static DashboardViewItem MasterItem(this DashboardView view) 
            => view.Items.OfType<DashboardViewItem>().First(item => item.Model.ActionsToolbarVisibility!=ActionsToolbarVisibility.Hide);

        internal static IEnumerable<T> Objects<T>(this CollectionSourceBase collectionSourceBase) {
            if (collectionSourceBase.Collection is IEnumerable collection)
                return collection.Cast<T>();
            if (collectionSourceBase.Collection is IListSource listSource)
                return listSource.GetList().Cast<T>();
            if (collectionSourceBase is PropertyCollectionSource propertyCollectionSource) {
                var masterObject = propertyCollectionSource.MasterObject;
                return masterObject != null ? ((IEnumerable)propertyCollectionSource.MemberInfo.GetValue(masterObject)).Cast<T>() : Enumerable.Empty<T>();
            }
            return collectionSourceBase.Collection is QueryableCollection queryableCollection
                ? ((IEnumerable<T>)queryableCollection.Queryable).ToArray() : throw new NotImplementedException($"{collectionSourceBase}");
        }
        
        internal static IEnumerable<T> Objects<T>(this View view) 
            => view is DetailView ? ((T)view.CurrentObject).YieldItem().ToArray()
                : view.ToListView().CollectionSource.Objects<T>();
        internal static CompositeView ToCompositeView(this View view) => (CompositeView)view ;
        internal static IEnumerable<NestedFrame> ToFrame(this IEnumerable<DashboardViewItem> source)
            => source.Select(item => (NestedFrame)item.Frame);

        internal static IEnumerable<DashboardViewItem> Items(this DashboardView dashboardView,params ViewType[] viewTypes)
            => dashboardView.GetItems<DashboardViewItem>().Where(item =>viewTypes.Length==0|| viewTypes.Any(viewType =>item.Model.View.Is(viewType) ));

        internal static bool Is(this IModelView modelView,ViewType viewType) 
            => viewType == ViewType.Any || (viewType == ViewType.DetailView ? modelView is IModelDetailView :
                viewType == ViewType.ListView ? modelView is IModelListView : modelView is IModelDashboardView);

        internal static IEnumerable<NestedFrame> NestedFrames(this DashboardView dashboardView,params ViewType[] viewTypes)
            => dashboardView.Items(viewTypes).ToFrame();
        internal static IEnumerable<NestedFrame> NestedFrames<TView>(this DashboardView dashboardView,params Type[] objectTypes) where TView:View 
            => dashboardView.GetItems<DashboardViewItem>()
                .Where(item => item.InnerView is TView && (!objectTypes.Any()||objectTypes.Contains(item.InnerView.ObjectTypeInfo.Type)))
                .ToFrame();

        internal static IEnumerable<TControl> Controls<TControl>(this CompositeView compositeView) 
            => compositeView.GetItems<ControlViewItem>().Select(item => item.Control)
                .OfType<TControl>();

        internal static T SetCurrentObject<T>(this View detailView, T currentObject) where T : class 
            => (T)(detailView.CurrentObject = detailView.ObjectSpace.GetObject(currentObject));

        internal static bool Is(this View view, Type objectType ) 
            => view.Is(ViewType.Any,Nesting.Any,objectType);
        internal static DetailView ToDetailView(this View view) => (DetailView)view;
        internal static ListView ToListView(this View view) => ((ListView)view);
        internal static bool Is(this View view, ViewType viewType = ViewType.Any, Nesting nesting = Nesting.Any, Type objectType = null) 
            => view.FitsCore( viewType) && view.FitsCore( nesting) &&
               (viewType==ViewType.DashboardView&&view is DashboardView||(objectType ?? typeof(object)).IsAssignableFrom(view.ObjectTypeInfo?.Type));

        private static bool FitsCore(this View view, ViewType viewType) 
            => view != null && (viewType == ViewType.ListView ? view is ListView : viewType == ViewType.DetailView
                    ? view is DetailView : viewType != ViewType.DashboardView || view is DashboardView);

        private static bool FitsCore(this View view, Nesting nesting) 
            => nesting == Nesting.Nested ? !view.IsRoot : nesting != Nesting.Root || view.IsRoot;
    }
}