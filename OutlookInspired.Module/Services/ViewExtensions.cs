using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Templates;

namespace OutlookInspired.Module.Services{
    
    public static class ViewExtensions{
        

        public static CompositeView ToCompositeView(this View view) => (CompositeView)view ;

        public static NestedFrame MasterFrame(this DashboardView view)
            => view.Items.OfType<DashboardViewItem>().Where(item => item.Model.ActionsToolbarVisibility!=ActionsToolbarVisibility.Hide)
                .Select(item => item.Frame).Cast<NestedFrame>().First();
        
        public static IEnumerable<DashboardViewItem> Items(this DashboardView dashboardView,params ViewType[] viewTypes)
            => dashboardView.GetItems<DashboardViewItem>().Where(item =>viewTypes.Length==0|| viewTypes.Any(viewType =>item.Model.View.Is(viewType) ));

        public static bool Is(this IModelView modelView,ViewType viewType) 
            => viewType == ViewType.Any || (viewType == ViewType.DetailView ? modelView is IModelDetailView :
                viewType == ViewType.ListView ? modelView is IModelListView : modelView is IModelDashboardView);

        public static IEnumerable<NestedFrame> NestedFrames(this DashboardView dashboardView,params ViewType[] viewTypes)
            => dashboardView.Items(viewTypes).Select(item => item.Frame).Cast<NestedFrame>();
        public static IEnumerable<NestedFrame> NestedFrames<TView>(this DashboardView dashboardView,params Type[] objectTypes) where TView:View 
            => dashboardView.GetItems<DashboardViewItem>()
                .Where(item => item.InnerView is TView && (!objectTypes.Any()||objectTypes.Contains(item.InnerView.ObjectTypeInfo.Type)))
                .Select(item => item.Frame).Cast<NestedFrame>();

        public static IEnumerable<TControl> Controls<TControl>(this CompositeView compositeView) 
            => compositeView.GetItems<ControlViewItem>().Select(item => item.Control)
                .OfType<TControl>();

        public static T SetCurrentObject<T>(this View detailView, T currentObject) where T : class 
            => (T)(detailView.CurrentObject = detailView.ObjectSpace.GetObject(currentObject));

        public static bool Is(this View view, Type objectType ) 
            => view.Is(ViewType.Any,Nesting.Any,objectType);
        public static DetailView ToDetailView(this View view) => (DetailView)view;
        public static ListView ToListView(this View view) => ((ListView)view);
        public static bool Is(this View view, ViewType viewType = ViewType.Any, Nesting nesting = Nesting.Any, Type objectType = null) 
            => view.FitsCore( viewType) && view.FitsCore( nesting) &&
               (viewType==ViewType.DashboardView&&view is DashboardView||(objectType ?? typeof(object)).IsAssignableFrom(view.ObjectTypeInfo?.Type));

        private static bool FitsCore(this View view, ViewType viewType) 
            => view != null && (viewType == ViewType.ListView ? view is ListView : viewType == ViewType.DetailView
                    ? view is DetailView : viewType != ViewType.DashboardView || view is DashboardView);

        private static bool FitsCore(this View view, Nesting nesting) 
            => nesting == Nesting.Nested ? !view.IsRoot : nesting != Nesting.Root || view.IsRoot;
    }
}