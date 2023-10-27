
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Win.Layout;
using DevExpress.XtraGrid;
using DevExpress.XtraLayout;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using ListView = DevExpress.ExpressApp.ListView;

namespace XAF.Testing.Win.XAF{
    public static class ViewExtensions{
        public static IObservable<GridControl> WhenGridControl(this DetailView detailView) 
            => detailView.WhenControlViewItemGridControl().SelectMany(control => control.WhenDataSourceChanged().To(control).StartWith(control));

        public static IObservable<T> SelectObject<T>(this ListView view, params T[] objects) where T : class => view.Defer(() => {
                var gridView = (view.Editor as GridListEditor)?.GridView;
                if (gridView == null)
                    throw new NotImplementedException(nameof(ListView.Editor));
                gridView.ClearSelection();
                return objects.ToNowObservable()
                    .SwitchIfEmpty(Observable.Defer(() => gridView.GetRow(gridView.GetRowHandle(0)).Observe()))
                    .SelectMany(obj => gridView.WhenSelectRow(obj))
                    .Select(_ => gridView.FocusRowObject(view.ObjectSpace, view.ObjectTypeInfo.Type) as T);
            });

        

        public static void ClearFilter(this ListView listView){
            if ((listView.Editor) is GridListEditor listViewEditor) listViewEditor.GridView.ActiveFilterCriteria = null;
        }

        public static IObservable<(IModelViewLayoutElement model,object control,ViewItem viewItem)> WhenItemCreated(this WinLayoutManager layoutManager) 
            => layoutManager.WhenEvent(nameof(WinLayoutManager.ItemCreated)).Select(p => p.EventArgs).Cast<ItemCreatedEventArgs>()
                .Select(e => (e.ModelLayoutElement,(object)e.Item, e.ViewItem));
        
        public static IObservable<ITabControlProvider> WhenTabControl(this DetailView detailView, IModelViewLayoutElement element) 
            => ((WinLayoutManager)detailView.LayoutManager).WhenItemCreated().Where(t => t.model == element).Select(t => t.control).Take(1)
                .SelectMany(tabbedControlGroup => detailView.LayoutManager.WhenLayoutCreated().Take(1).To(tabbedControlGroup))
                .Select(o => new TabControlProvider((TabbedControlGroup)o));

        
        public static IObservable<GridControl> WhenControlViewItemGridControl(this DetailView detailView)
            => detailView.WhenControlViewItemWinControl<GridControl>();

        public static IObservable<(TItem item, Control control)> WhenWinControl<TItem>(this IEnumerable<TItem> source,Type controlType) where TItem:ViewItem 
            => source.ToNowObservable()
                .SelectMany(item => item.WhenControlCreated().Select(_ => item.Control).StartWith(item.Control).WhenNotDefault().Cast<Control>().Take(1)
                    .SelectMany(control => control.Controls.Cast<Control>().Prepend(control).ToNowObservable().WhenNotDefault().Where(controlType.IsInstanceOfType).Take(1))
               
                .Select(control => (item,control)));
        
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