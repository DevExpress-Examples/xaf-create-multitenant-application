using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Layout;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace XAF.Testing.Win.XAF{
    public class TabControlObserver:ITabControlObserver{
        public IObservable<object> WhenTabControl(DetailView detailView, IModelViewLayoutElement element) 
            => ((WinLayoutManager)detailView.LayoutManager).WhenItemCreated().Where(t => t.model == element).Select(t => t.control).Take(1)
                .SelectMany(tabbedControlGroup => detailView.LayoutManager.WhenLayoutCreated().Take(1).To(tabbedControlGroup));
    }
}