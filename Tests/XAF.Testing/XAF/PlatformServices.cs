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
    

    [Obsolete("check EmployeeExtensions not use this")]
    public interface ITabControlAsserter{
        IObservable<ITabControlProvider> AssertTabbedGroup(Type objectType, int tabPages);
    }

    public interface IRichEditControlAsserter{
        IObservable<Unit> Assert(DetailView detailView, bool assertMailMerge);    
    }
    public interface IPdfViewerAssertion{
        IObservable<Unit> Assert(DetailView detailView);
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
        IObservable<(Frame frame, object o)> WhenObjects(Frame frame, int count = 0);
    }

    public interface IUserControlProvider{
        IObservable<object> WhenGridControl(DetailView detailView);
    }

    public interface IUserControlProperties{
        int ObjectsCount(object control);
    }
    
    public interface INewRowAdder{
        void AddNewRowAndCloneMembers(Frame frame, object existingObject);
    }
    
    
    public interface IReportAssertion{
        IObservable<Unit> Assert(Frame frame, string item);
    }

    public interface ISelectedObjectProcessor{
        IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(Frame listViewFrame);
    }


    public interface IMapsControlAssertion:IViewItemControlAssertion{
        
    }
    public interface IViewItemControlAssertion{
        IObservable<Unit> Assert(DetailView detailView);
    }

    public interface IWindowMaximizer{
        IObservable<Window> WhenMaximized(Window window);
    }


}