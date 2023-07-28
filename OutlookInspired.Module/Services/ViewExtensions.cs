using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;

namespace OutlookInspired.Module.Services{
    internal static class ViewExtensions{
        public static IEnumerable<View> Views(this DashboardView dashboardView)
            => dashboardView.Views<View>();
        public static IEnumerable<TView> Views<TView>(this DashboardView dashboardView) where TView:View
            => dashboardView.GetItems<DashboardViewItem>().Select(item => item.InnerView).OfType<TView>();
        public static IEnumerable<NestedFrame> NestedFrames<TView>(this DashboardView dashboardView,params Type[] objectTypes) where TView:View 
            => dashboardView.GetItems<DashboardViewItem>()
                .Where(item => item.InnerView is TView && (!objectTypes.Any()||objectTypes.Contains(item.InnerView.ObjectTypeInfo.Type)))
                .Select(item => item.Frame).Cast<NestedFrame>();

        public static IEnumerable<TControl> Controls<TControl>(this CompositeView compositeView) 
            => compositeView.GetItems<ControlViewItem>().Select(item => item.Control)
                .OfType<TControl>();

        public static T SetCurrentObject<T>(this DetailView detailView, T currentObject) where T : class 
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