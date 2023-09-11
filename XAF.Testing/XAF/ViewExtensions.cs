using System.Collections;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid;
using XAF.Testing.RX;
using ListView = DevExpress.ExpressApp.ListView;
using View = DevExpress.ExpressApp.View;

namespace XAF.Testing.XAF{
    public static class ViewExtensions{
        public static IEnumerable<NestedFrame> ToFrame(this IEnumerable<DashboardViewItem> source)
            => source.Select(item => item.Frame).Cast<NestedFrame>();
        public static IObservable<NestedFrame> ToFrame(this IObservable<DashboardViewItem> source)
            => source.Select(item => item.Frame).Cast<NestedFrame>();

        public static IObservable<object> WhenPropertyEditorControl(this DetailView detailView)
            => detailView.WhenViewItemControl<PropertyEditor>();
        public static IObservable<object> WhenViewItemControl<T>(this DetailView detailView) where T:ViewItem 
            => detailView.GetItems<T>().ToNowObservable()
                .SelectMany(editor => editor.WhenControlCreated().Select(propertyEditor => propertyEditor.Control).StartWith(editor.Control).WhenNotDefault());
        
        public static IObservable<T> WhenClosing<T>(this T view) where T : View 
            => view.WhenViewEvent(nameof(view.Closing)).To(view).Select(view1 => view1);
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
        public static IObservable<object> WhenObjects(this ListView listView) 
            => listView.Objects().ToNowObservable()
                .MergeToObject(listView.CollectionSource.WhenCollectionChanged()
                    .SelectMany(_ => listView.Objects()))
                .MergeToObject(listView.CollectionSource.WhenCriteriaApplied().SelectMany(@base => @base.Objects() ));
        
        public static IEnumerable<object> Objects(this View view) => view.Objects<object>();
        public static CompositeView ToCompositeView(this View view) => (CompositeView)view ;
        public static DashboardView ToDashboardView(this View view) => (DashboardView)view ;
        public static IObservable<IFrameContainer> NestedFrameContainers<TView>(this TView view, params Type[] objectTypes ) where TView : CompositeView  
            => view.GetItems<IFrameContainer>().ToNowObservable().OfType<ViewItem>()
                .SelectMany(item => item.WhenControlCreated().StartWith(item.Control).WhenNotDefault().Take(1).To(item).Cast<IFrameContainer>())
                .NestedFrameContainers(view, objectTypes);

        public static (ITypeInfo typeInfo, object keyValue) CurrentObjectInfo(this View view) 
            => (view.ObjectTypeInfo,view.ObjectSpace.GetKeyValue(view.CurrentObject));

        public static IEnumerable<(string name, object value)> CloneExistingObjectMembers(this CompositeView compositeView,bool inLineEdit, object existingObject = null) 
            => compositeView is DetailView detailView?detailView.CloneExistingObjectMembers(existingObject).IgnoreElements()
                    .Select(_ => default((string name, object value))) :compositeView.ToListView().CloneExistingObjectMembers(inLineEdit,existingObject);

        public static IEnumerable<Unit> CloneExistingObjectMembers(this DetailView compositeView, object existingObject = null){
            existingObject ??= compositeView.ObjectSpace.FindObject(compositeView.ObjectTypeInfo.Type);
            return compositeView.ObjectTypeInfo.Members.Where(memberInfo => !memberInfo.IsKey&&!memberInfo.IsList)
                .Do(memberInfo => {
                    var existingValue = memberInfo.GetValue(existingObject);
                    memberInfo.SetValue(compositeView.CurrentObject, memberInfo.IsPersistent?compositeView.ObjectSpace.GetObject(existingValue): existingValue);
                })
                .IgnoreElements().ToUnit();
        }
        public static IEnumerable<(string name, object value)> CloneExistingObjectMembers(this ListView listView,bool inLineEdit, object existingObject = null){
            existingObject ??= listView.Objects().First();
            if (inLineEdit){
                return listView.ToListView().Model.MemberViewItems().Where(item => !item.ModelMember.MemberInfo.IsKey)
                    .Select(item =>(item.ModelMember.MemberInfo.Name,item.ModelMember.MemberInfo.GetValue(existingObject)) );    
            }
            return listView.CloneExistingObjectMembers(true,existingObject)
                .Do(t => listView.EditView.ObjectTypeInfo.FindMember(t.name).SetValue(listView.EditView.CurrentObject,t.value)).ToArray();
        }

        

        public static bool IsNewObject(this CompositeView compositeView)
            => compositeView.ObjectSpace.IsNewObject(compositeView.CurrentObject);
        public static IEnumerable<TView> Views<TView>(this DashboardView dashboardView) where TView:View
            => dashboardView.GetItems<DashboardViewItem>().Select(item => item.InnerView).OfType<TView>();
        static IObservable<T> SelectObject<T>(this IObservable<ListView> source,params T[] objects) where T : class 
            => source.SelectMany(view => {
                var gridView = (view.Editor as GridListEditor)?.GridView;
                return gridView == null
                    ? throw new NotImplementedException(nameof(view.Editor))
                    : objects.ToNowObservable().SelectMany(obj => gridView.WhenSelectRow(obj))
                        .Select(_ => gridView.FocusRowObject(view.ObjectSpace, view.ObjectTypeInfo.Type) as T);
            });

        public static IObservable<object> SelectObject(this ListView listView, params object[] objects)
            => listView.SelectObject<object>(objects);
        
        public static IObservable<TO> SelectObject<TO>(this ListView listView,params TO[] objects) where TO : class 
            => listView.Editor.WhenControlsCreated()
                .SelectMany(editor => editor.Control.WhenEvent("DataSourceChanged").StartWith(editor.Control.GetPropertyValue("DataSource")).WhenNotDefault()).To(listView)
                .SelectObject(objects);

        public static IObservable<T> WhenControlsCreated<T>(this T view) where T : View 
            => view.WhenViewEvent(nameof(View.ControlsCreated));
        public static IObservable<T> WhenCurrentObjectChanged<T>(this T view) where T:View 
            => view.WhenViewEvent(nameof(View.CurrentObjectChanged)).To(view);
        public static IObservable<T> WhenSelectionChanged<T>(this T view, int waitUntilInactiveSeconds = 0) where T : View
            => view.WhenViewEvent(nameof(View.SelectionChanged)).To(view)
                .Publish(changed => waitUntilInactiveSeconds > 0 ? changed.WaitUntilInactive(waitUntilInactiveSeconds) : changed);
        
        public static IObservable<ListPropertyEditor> NestedListViews<TView>(this TView view, params Type[] objectTypes ) where TView : DetailView 
            => view.NestedViewItems<TView,ListPropertyEditor>(objectTypes);

        public static IObservable<object> WhenTabControl(this DetailView detailView, Func<IModelTabbedGroup, bool> match=null)
            => detailView.WhenTabControl(detailView.LayoutGroupItem(element => element is IModelTabbedGroup group&& (match?.Invoke(group) ?? true)));

        public static IObservable<TView> When<TView>(this IObservable<TView> source,string viewId) where TView:View 
            => source.Where(view =>view.Id==viewId);
        
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

        internal static ListView AsListView(this View view) => view as ListView;
        internal static ListView ToListView(this View view) => ((ListView)view);

        public static IObservable<T> WhenControlsCreated<T>(this IObservable<T> source) where T:View 
            => source.SelectMany(view => view.WhenViewEvent(nameof(View.ControlsCreated)));
        public static IObservable<object> WhenSelectedObjects(this View view) 
            => view.WhenSelectionChanged().SelectMany(_ => view.SelectedObjects.Cast<object>())
                .StartWith(view.SelectedObjects.Cast<object>());
        public static IObservable<object> WhenObjects(this View view) 
            => view is ListView listView?listView.CollectionSource.WhenCollectionChanged().SelectMany(_ => listView.Objects())
                .StartWith(listView.Objects()):view.ToDetailView().WhenCurrentObjectChanged()
                .Select(detailView => detailView.CurrentObject).StartWith(view.CurrentObject).WhenNotDefault();

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

        public static IObservable<Frame> ToEditFrame(this IObservable<ListView> source) 
            => source.Select(view => view.EditFrame);

        public static IObservable<GridControl> WhenControlViewItemGridControl(this DetailView detailView)
            => detailView.WhenControlViewItemWinControl<GridControl>();

        public static IObservable<(TItem item, Control control)> WhenWinControl<TItem>(this IEnumerable<TItem> source,Type controlType) where TItem:ViewItem 
            => source.ToNowObservable()
                .SelectMany(item => item.WhenControlCreated().Select(_ => item.Control).StartWith(item.Control).WhenNotDefault().Cast<Control>()
                    .SelectMany(control => control.Controls.Cast<Control>().Prepend(control))
                    .WhenNotDefault().Where(controlType.IsInstanceOfType)
                    .Select(control => (item,control)))
                ;
        
        public static IObservable<T> WhenControlViewItemWinControl<T>(this DetailView detailView) where T:Control 
            => detailView.GetItems<ControlViewItem>().WhenWinControl(typeof(T)).Select(t => t.control).Cast<T>();
        public static IObservable<T> WhenViewItemWinControl<T>(this DetailView detailView) where T:Control 
            => detailView.GetItems<ViewItem>().WhenWinControl(typeof(T)).Select(t => t.control).Cast<T>();
        public static IObservable<(ViewItem item, Control control)> WhenViewItemWinControl(this DetailView detailView,Type controlType)  
            => detailView.WhenViewItemWinControl<ViewItem>(controlType);
        public static IObservable<(TItem item, Control control)> WhenViewItemWinControl<TItem>(this DetailView detailView,Type controlType) where TItem:ViewItem  
            => detailView.GetItems<TItem>().WhenWinControl(controlType);    

    }
}