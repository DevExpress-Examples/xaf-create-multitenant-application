using System.Collections;
using System.ComponentModel;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Persistent.Validation;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class ViewExtensions{
        internal static bool Is(this View view, ViewType viewType = ViewType.Any, Nesting nesting = Nesting.Any, Type objectType = null) 
            => view.FitsCore( viewType) && view.FitsCore( nesting) &&
               (viewType==ViewType.DashboardView&&view is DashboardView||(objectType ?? typeof(object)).IsAssignableFrom(view.ObjectTypeInfo?.Type));

        private static bool FitsCore(this View view, ViewType viewType) 
            => view != null && (viewType == ViewType.ListView ? view is ListView : viewType == ViewType.DetailView
                ? view is DetailView : viewType != ViewType.DashboardView || view is DashboardView);

        private static bool FitsCore(this View view, Nesting nesting) 
            => nesting == Nesting.Nested ? !view.IsRoot : nesting != Nesting.Root || view.IsRoot;

        public static bool Is(this View view, params Type[] objectTypes ) 
            => objectTypes.All(objectType => view.Is(ViewType.Any,Nesting.Any,objectType));
        
        public static IEnumerable<object> Objects(this CollectionSourceBase collectionSourceBase) => collectionSourceBase.Objects<object>();

        public static IEnumerable<T> Objects<T>(this CollectionSourceBase collectionSourceBase) {
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
        
        public static IEnumerable<T> Objects<T>(this View view) 
            => view is DetailView ? ((T)view.CurrentObject).YieldItem().ToArray()
                : view.ToListView().CollectionSource.Objects<T>();
       
        
        public static IObservable<DashboardViewItem> When(this IObservable<DashboardViewItem> source, params ViewType[] viewTypes) 
            => source.Where(item => viewTypes.All(viewType => item.InnerView.Is(viewType)));
        
        public static IEnumerable<object> Objects(this View view) => view.Objects<object>();
        public static CompositeView ToCompositeView(this View view) => (CompositeView)view ;
        public static DashboardView ToDashboardView(this View view) => (DashboardView)view ;
        public static IObservable<IFrameContainer> NestedFrameContainers<TView>(this TView view, params Type[] objectTypes ) where TView : CompositeView  
            => view.GetItems<IFrameContainer>().ToNowObservable().OfType<ViewItem>()
                .SelectMany(item => item.WhenControlCreated().StartWith(item.Control).WhenNotDefault().Take(1).To(item).Cast<IFrameContainer>())
                .NestedFrameContainers(view, objectTypes);

        public static IEnumerable<PropertyEditor> CloneRequiredMembers(this CompositeView compositeView,object existingObject=null) {
            existingObject ??= compositeView.ObjectSpace.FindObject(compositeView.ObjectTypeInfo.Type);
            return compositeView.GetItems<PropertyEditor>().Where(editor =>
                    editor.MemberInfo.FindAttributes<RuleRequiredFieldAttribute>().Any())
                .Do(editor => { editor.MemberInfo.SetValue(compositeView.CurrentObject, editor.MemberInfo.GetValue(existingObject)); });
        }

        public static bool IsNewObject(this CompositeView compositeView)
            => compositeView.ObjectSpace.IsNewObject(compositeView.CurrentObject);
        public static IEnumerable<TView> Views<TView>(this DashboardView dashboardView) where TView:View
            => dashboardView.GetItems<DashboardViewItem>().Select(item => item.InnerView).OfType<TView>();
        static IObservable<T> SelectObject<T>(this IObservable<ListView> source,params T[] objects) where T : class 
            => source.SelectMany(view => {
                var gridView = (view.Editor as GridListEditor)?.GridView;
                if (gridView != null){
                    return objects.ToNowObservable().Select(row => {
                            var rowHandle = gridView.FindRow(row);
                            gridView.SelectRow(rowHandle);
                            return rowHandle;
                        })
                        .BufferUntilCompleted()
                        .Select(obj => {
                            if (obj.Length==1){
                                gridView.FocusedRowHandle = obj.First();
                            }
                            return gridView.FocusedRowObject as T;
                        });
                }
                throw new NotImplementedException(nameof(view.Editor));
            });

        public static IObservable<object> SelectObject(this ListView listView, params object[] objects)
            => listView.SelectObject<object>(objects);
        
        public static IObservable<TO> SelectObject<TO>(this ListView listView,params TO[] objects) where TO : class 
            => listView.Editor.WhenControlsCreated()
                .SelectMany(editor => editor.Control.WhenEvent("DataSourceChanged")).To(listView)
                .SelectObject(objects);

        public static IObservable<T> WhenControlsCreated<T>(this T view) where T : View 
            => view.WhenViewEvent(nameof(View.ControlsCreated));
        
        public static IObservable<T> WhenSelectionChanged<T>(this T view, int waitUntilInactiveSeconds = 0) where T : View
            => view.WhenViewEvent(nameof(View.SelectionChanged)).To(view)
                .Publish(changed => waitUntilInactiveSeconds > 0 ? changed.WaitUntilInactive(waitUntilInactiveSeconds) : changed);
        
        public static IObservable<ListPropertyEditor> NestedListViews<TView>(this TView view, params Type[] objectTypes ) where TView : DetailView 
            => view.NestedViewItems<TView,ListPropertyEditor>(objectTypes);

        public static IObservable<object> WhenTabControl(this DetailView detailView, Func<IModelTabbedGroup, bool> match=null)
            => detailView.WhenTabControl(detailView.LayoutGroupItem(element => element is IModelTabbedGroup group&& (match?.Invoke(group) ?? true)));

        public static IModelViewLayoutElement LayoutGroupItem(this DetailView detailView,Func<IModelViewLayoutElement,bool> match)
            => detailView.Model.Layout.Flatten().FirstOrDefault(match);

        public static IObservable<object> WhenTabControl(this DetailView detailView, IModelViewLayoutElement element)
            => detailView.LayoutManager.WhenItemCreated().Where(t => t.model == element).Select(t => t.control).Take(1)
                .SelectMany(tabbedControlGroup => detailView.LayoutManager.WhenLayoutCreated().Take(1).To(tabbedControlGroup));
        public static IObservable<TViewItem> NestedViewItems<TView,TViewItem>(this TView view, params Type[] objectTypes ) where TView : DetailView where TViewItem:ViewItem,IFrameContainer 
            => view.NestedFrameContainers(objectTypes).OfType<TViewItem>();
        
        public static IObservable<T> WhenClosed<T>(this T view) where T : View 
            => view.WhenViewEvent(nameof(view.Closed));
        public static IObservable<T> WhenActivated<T>(this T view) where T : View 
            => view.WhenViewEvent(nameof(View.Activated));
        public static IObservable<TView> WhenViewEvent<TView>(this TView view,string eventName) where TView:View 
            => view.WhenEvent(eventName).To(view).TakeUntilViewDisposed();
        
        public static IObservable<TView> TakeUntilViewDisposed<TView>(this IObservable<TView> source) where TView:View 
            => source.TakeWhileInclusive(view => !view.IsDisposed);
        public static ListView ToListView(this View view) => ((ListView)view);
        
        
        private static IObservable<TFrameContainer> NestedFrameContainers<TView,TFrameContainer>(this IObservable<TFrameContainer> lazyListPropertyEditors, TView view, Type[] objectTypes) where TView : CompositeView where TFrameContainer:IFrameContainer{
            var listFrameContainers = view.GetItems<ViewItem>().OfType<TFrameContainer>().Where(editor => editor.Frame?.View != null)
                .ToNowObservable().Merge(lazyListPropertyEditors);
            var nestedEditors = listFrameContainers.WhenNotDefault(container => container.Frame).SelectMany(frameContainer => {
                var detailView =frameContainer.Frame.View is ListView listView? listView.EditView:null;
                return detailView != null ? detailView.NestedFrameContainers(objectTypes).OfType<TFrameContainer>() : Observable.Never<TFrameContainer>();
            });
            return listFrameContainers.WhenNotDefault(container => container.Frame)
                .Where(frameContainer =>!objectTypes.Any()|| objectTypes.Any(type => type.IsAssignableFrom(frameContainer.Frame.View.ObjectTypeInfo.Type)))
                .Merge(nestedEditors);
        }
        
        

    }
}