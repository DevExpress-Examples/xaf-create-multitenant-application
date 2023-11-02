using System.Reactive;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace XAF.Testing.XAF{
    public interface ITabControlProvider{
        object TabControl{ get; }
        int TabPages{ get; }
        void SelectTab(int pageIndex);
    }
    
    public interface IRichEditControlAsserter{
        IObservable<Unit> Assert(DetailView detailView, bool assertMailMerge);    
    }
    public interface IPdfViewerAssertion:IAssertViewItemClientIsReady{
        
    }
    public interface IDashboardViewGridControlDetailViewObjectsAsserter{
        IObservable<Frame> AssertDashboardViewGridControlDetailViewObjects(Frame frame, params string[] relationNames);
    }
    public interface IFilterClearer{
        void Clear(ListView listView);
    }

    public interface IDocumentActionAssertion{
        IObservable<Frame> Assert(SingleChoiceAction action, ChoiceActionItem item);
    }

    public interface ITabControlObserver{
        IObservable<ITabControlProvider> WhenTabControl(DetailView detailView, IModelViewLayoutElement element);
    }

    public interface IDataSourceChanged{
        IObservable<EventPattern<object>> WhenDatasourceChanged(object editor);
    }
    public interface IDashboardColumnViewObjectSelector{
        IObservable<Unit> SelectDashboardColumnViewObject(DashboardViewItem frame);
    }

    public interface IObjectSelector<T> where T : class{
        IObservable<T> SelectObject(ListView source, params T[] objects);
    }

    public interface IFrameObjectObserver{
        IObservable<object> WhenObjects(Frame frame, int count = 0);
    }

    public interface IUserControlProvider{
        IObservable<object> WhenGridControl(DetailView detailView);
    }

    public interface IUserControlObjects{
        int ObjectsCount(object control);
        IObservable<object> WhenObjects(object control, int i);
    }
    
    public interface INewRowAdder{
        void AddNewRowAndCloneMembers(Frame frame, object existingObject);
    }
    
    
    public interface IAssertReport{
        IObservable<Unit> Assert(Frame frame, string item);
    }

    public interface ISelectedObjectProcessor{
        IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(Frame listViewFrame);
    }


    public interface IAssertMapControl:IAssertViewItemClientIsReady{
        
    }
    public interface IAssertViewItemClientIsReady{
        IObservable<Unit> Assert(DetailView detailView);
    }

    public interface IWindowMaximizer{
        IObservable<Window> WhenMaximized(Window window);
    }


}