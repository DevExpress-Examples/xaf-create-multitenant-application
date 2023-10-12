using System.Reactive;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace XAF.Testing.Win.XAF{
    public class DashboardColumnViewObjectSelector : IDashboardColumnViewObjectSelector{
        public IObservable<Unit> SelectDashboardColumnViewObject(Frame frame, Func<DashboardViewItem, bool> itemSelector = null) 
            => frame.DashboardViewItems(ViewType.DetailView).Where(itemSelector ?? (_ => true)).ToNowObservable().SelectDashboardColumnViewObject().ToUnit();
    }
}