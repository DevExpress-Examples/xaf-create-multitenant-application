using System.Reactive;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace XAF.Testing.XAF{
    public interface ITabControlProvider{
        object TabControl{ get; }
        int TabPages{ get; }
        void SelectTab(int pageIndex);
    }
    public interface ITabControlAsserter{
        IObservable<ITabControlProvider> AssertTabbedGroup(Type objectType, int tabPages);
    }

    public interface ITabControlObserver{
        IObservable<ITabControlProvider> WhenTabControl(DetailView detailView, IModelViewLayoutElement element);
    }
    
    public interface IDashboardColumnViewObjectSelector{
        IObservable<Unit> SelectDashboardColumnViewObject(Frame frame, Func<DashboardViewItem, bool> itemSelector = null);
    }

    public interface IObjectSelector<T> where T : class{
        IObservable<T> SelectObject(ListView source, params T[] objects);
    }

    public interface IFrameObjectObserver{
        IObservable<(Frame frame, object o)> WhenObjects(Frame frame, int count = 0);
    }

    public interface INewObjectController{
        IObservable<Frame> CreateNewObjectController(Frame frame);
    }

    public interface INewRowAdder{
        void AddNewRowAndCloneMembers(Frame frame, object existingObject);
    }
    
    public interface IReportAsserter{
        IObservable<Unit> AssertReport(Frame frame, string item);
    }

    public interface ISelectedObjectProcessor{
        IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(Frame listViewFrame);
    }

}