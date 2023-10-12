using System.Reactive.Linq;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Layout;
using XAF.Testing.RX;

namespace XAF.Testing.Win.XAF{
    public static class LayoutManagerExtensions{
        public static IObservable<(IModelViewLayoutElement model,object control,ViewItem viewItem)> WhenItemCreated(this WinLayoutManager layoutManager) 
            => layoutManager.WhenEvent(nameof(WinLayoutManager.ItemCreated)).Select(p => p.EventArgs).Cast<ItemCreatedEventArgs>()
                .Select(e => (e.ModelLayoutElement,(object)e.Item, e.ViewItem));
    }
}