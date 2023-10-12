
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.XtraGrid;
using XAF.Testing.RX;

namespace XAF.Testing.Win.XAF{
    public static class ViewExtensions{
        public static IObservable<GridControl> WhenControlViewItemGridControl(this DetailView detailView)
            => detailView.WhenControlViewItemWinControl<GridControl>();

        public static IObservable<(TItem item, Control control)> WhenWinControl<TItem>(this IEnumerable<TItem> source,Type controlType) where TItem:ViewItem 
            => source.ToNowObservable().Do(item => throw new NotImplementedException()).To<(TItem item, Control control)>()
                // .SelectMany(item => item.WhenControlCreated().Select(_ => item.Control).StartWith(item.Control).WhenNotDefault().Cast<Control>()
                //     .SelectMany(control => control.Controls.Cast<Control>().Prepend(control))
                //     .WhenNotDefault().Where(controlType.IsInstanceOfType)
                //     .Select(control => (item,control)))
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