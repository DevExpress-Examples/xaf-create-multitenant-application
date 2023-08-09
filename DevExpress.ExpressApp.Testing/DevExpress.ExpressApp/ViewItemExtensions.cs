using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Testing.RXExtensions;

namespace DevExpress.ExpressApp.Testing.DevExpress.ExpressApp{
    public static class ViewItemExtensions{
        public static IObservable<TTabbedControl> WhenTabControl<TTabbedControl>(this IObservable<DashboardViewItem> source) 
            => source.SelectMany(item => item.Frame.View.ToDetailView().WhenTabControl().Cast<TTabbedControl>());
        public static IObservable<TView> ToView<TView>(this IObservable<DashboardViewItem> source)
            => source.Select(item => item.Frame.View).Cast<TView>();
        public static IObservable<TView> OfView<TView>(this IObservable<DashboardViewItem> source)
            => source.Select(item => item.Frame.View).OfType<TView>();
        public static IObservable<T> WhenControlCreated<T>(this T source) where T:ViewItem 
            => source.Observe().ControlCreated();
        
        public static IObservable<T> ControlCreated<T>(this IObservable<T> source) where T:ViewItem
            => source.SelectMany(item => item.WhenEvent(nameof(ViewItem.ControlCreated))
                .Select(_ => item)).TakeUntilDisposed();
        
        public static IObservable<T> TakeUntilDisposed<T>(this IObservable<T> source) where T : ViewItem
            => source.TakeWhileInclusive(item => !item.IsDisposed());
        
        public static bool IsDisposed<T>(this T source) where T : ViewItem
            => (bool)source.GetPropertyValue("IsDisposed");
        
        public static IObservable<T> ControlCreated<T>(this IEnumerable<T> source) where T:ViewItem 
            => source.ToObservable(ImmediateScheduler.Instance).ControlCreated();
        
    }
}