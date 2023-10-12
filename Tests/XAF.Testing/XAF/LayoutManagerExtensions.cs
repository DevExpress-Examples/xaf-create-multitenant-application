using DevExpress.ExpressApp.Layout;
using XAF.Testing.RX;

namespace XAF.Testing.XAF{
    public static class LayoutManagerExtensions{
        public static IObservable<LayoutManager> WhenLayoutCreated(this LayoutManager layoutManager) 
            => layoutManager.WhenEvent(nameof(layoutManager.LayoutCreated)).To(layoutManager);
        
    }
}