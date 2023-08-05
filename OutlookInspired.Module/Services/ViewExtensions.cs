using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Templates;
using OutlookInspired.Module.Controllers;

namespace OutlookInspired.Module.Services{
    public enum MasterDetailType{
        BothViewsNative,
        MasterNative,
        Custom,
        MasterCustom,
        BothViewsCustom,
        ChildCustom
    }
    internal static class ViewExtensions{
        public static MasterDetailType MasterDetailType(this DashboardView dashboardView){
            var items = dashboardView.Items(ViewType.DetailView,ViewType.ListView).ToArray();
            if (items.All(item => item.IsMaster())){
                return Services.MasterDetailType.BothViewsCustom;
            }
            var masterItem = items.MasterItem();
            var childItem = items.Except(masterItem.YieldItem()).First();
            return masterItem.IsMaster() ? childItem.IsMaster()
                    ? Services.MasterDetailType.BothViewsCustom : Services.MasterDetailType.MasterCustom
                : childItem.IsMaster() ? Services.MasterDetailType.ChildCustom
                    : Services.MasterDetailType.BothViewsNative;
        }

        public static DashboardViewItem MasterItem(this DashboardViewItem[] items) 
            => items.First(item => item.Model.ActionsToolbarVisibility != ActionsToolbarVisibility.Hide);

        public static CompositeView ToCompositeView(this View view) => (CompositeView)view ;

        public static bool IsMaster(this DashboardViewItem item)
            => item.InnerView.IsMaster();

        public static NestedFrame MasterFrame(this DashboardView view)
            => view.Items.OfType<DashboardViewItem>().Where(item => item.Model.ActionsToolbarVisibility!=ActionsToolbarVisibility.Hide)
                .Select(item => item.Frame).Cast<NestedFrame>().First();
        
        public static bool IsMaster(this View view) => view is DetailView detailView && detailView.Model.IsMaster();

        static bool IsMaster(this IModelDetailView detailViewModel){
            var modelControlDetailItems = detailViewModel.Items.OfType<IModelControlDetailItem>().ToArray();
            return modelControlDetailItems.Length>0&& modelControlDetailItems.All(item => XafTypesInfo.Instance.FindTypeInfo(item.ControlTypeName)
                    .FindAttribute<DetailUserControlAttribute>() == null);
        }

        public static UserControlController SetSelectionContext(this UserControlController userControlController,View view){
            userControlController.Actions.ForEach(action => action.SelectionContext = view);
            return userControlController;
        }

        public static void DeleteSelectObjects(this ObjectView objectView)
            => objectView.SelectedObjects.Cast<object>().Do(o => objectView.ObjectSpace.Delete(o))
                .Finally(() => objectView.ObjectSpace.CommitChanges());
        
        public static IEnumerable<View> Views(this DashboardView dashboardView)
            => dashboardView.Views<View>();
        public static IEnumerable<TView> Views<TView>(this DashboardView dashboardView) where TView:View
            => dashboardView.GetItems<DashboardViewItem>().Select(item => item.InnerView).OfType<TView>();
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

        public static T SetCurrentObject<T>(this View detailView, T currentObject) where T : class{
            var viewCurrentObject = (T)(detailView.CurrentObject = detailView.ObjectSpace.GetObject(currentObject));
            // detailView.GetItems<ControlViewItem>().Select(item => item.Control).OfType<IUserControl>()
                // .ForEach(control => control.SetDataSource(viewCurrentObject));
            return viewCurrentObject;
        }

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