using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Win.Layout;
using DevExpress.Utils.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraLayout;
using Microsoft.Extensions.DependencyInjection;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using ListView = DevExpress.ExpressApp.ListView;

namespace XAF.Testing.Win.XAF{
    public static class PlatformImplementations{
        public static void AddPlatformServices(this IServiceCollection serviceCollection){
            serviceCollection.AddSingleton<ITabControlObserver, TabControlObserver>();
            serviceCollection.AddSingleton<IDashboardColumnViewObjectSelector, DashboardColumnViewObjectSelector>();
            serviceCollection.AddSingleton<IFrameObjectObserver, FrameObjectObserver>();
            serviceCollection.AddSingleton<INewObjectController, NewObjectController>();
            serviceCollection.AddSingleton<INewRowAdder, NewRowAdder>();
            serviceCollection.AddSingleton<IReportAsserter, ReportAsserter>();
            serviceCollection.AddSingleton<ISelectedObjectProcessor, SelectedObjectProcessor>();
            serviceCollection.AddSingleton(typeof(IObjectSelector<>), typeof(ObjectSelector<>));
        }
    }
    
    public class NewObjectController : INewObjectController{
        public IObservable<Frame> CreateNewObjectController(Frame frame) 
            => frame.View.WhenObjectViewObjects(1).Take(1)
                .SelectMany(selectedObject => frame.ColumnViewCreateNewObject().SwitchIfEmpty(frame.ListViewCreateNewObject())
                    .SelectMany(newObjectFrame => newObjectFrame.View.ToCompositeView().CloneExistingObjectMembers(false, selectedObject)
                        .Select(_ => default(Frame)).IgnoreElements().Concat(newObjectFrame.YieldItem())));
    }
    
    public class NewRowAdder : INewRowAdder{
        public void AddNewRowAndCloneMembers(Frame frame, object existingObject) 
            => ((GridListEditor)frame.View.ToListView().Editor).GridView
                .AddNewRow(frame.View.ToCompositeView().CloneExistingObjectMembers(true, existingObject).ToArray());
    }

    public class ObjectSelector<T> : IObjectSelector<T> where T : class{
        public IObservable<T> SelectObject(ListView view, params T[] objects) 
            => view.Defer(() => {
                var gridView = (view.Editor as GridListEditor)?.GridView;
                if (gridView == null)
                    throw new NotImplementedException(nameof(ListView.Editor));
                gridView.ClearSelection();
                return objects.ToNowObservable()
                    .SwitchIfEmpty(Observable.Defer(() => gridView.GetRow(gridView.GetRowHandle(0)).Observe()))
                    .SelectMany(obj => gridView.WhenSelectRow(obj))
                    .Select(_ => gridView.FocusRowObject(view.ObjectSpace, view.ObjectTypeInfo.Type) as T);
            });
    }
    
    public class ReportAsserter : IReportAsserter{
        public IObservable<Unit> AssertReport(Frame frame, string item) 
            => frame.AssertReport(item).ToUnit();
    }

    public class SelectedObjectProcessor : ISelectedObjectProcessor{
        public IObservable<(Frame frame, Frame detailViewFrame)> ProcessSelectedObject(Frame listViewFrame) 
            => listViewFrame.WhenGridControl()
                .Publish(source => source.SelectMany(t => listViewFrame.Application.WhenFrame(((NestedFrame)t.frame)
                            .DashboardChildDetailView().ObjectTypeInfo.Type, ViewType.DetailView)
                        .Where(frame1 => frame1.View.ObjectSpace.GetKeyValue(frame1.View.CurrentObject)
                            .Equals(((ColumnView)t.gridControl.MainView).FocusedRowObjectKey(frame1.View.ObjectSpace))))
                    .Merge(Observable.Defer(() =>
                        source.ToFirst().ProcessEvent(EventType.DoubleClick).To<Frame>().IgnoreElements())))
                .SwitchIfEmpty(listViewFrame.ProcessListViewSelectedItem())
                .Select(detailViewFrame => (frame: listViewFrame, detailViewFrame));
    }

    class TabControlProvider:ITabControlProvider{
        public TabControlProvider(TabbedControlGroup tabControl) => TabControl = tabControl;

        public object TabControl{ get; }
        public int TabPages => ((TabbedControlGroup)TabControl).TabPages.Count;
        public void SelectTab(int pageIndex) => ((TabbedControlGroup)TabControl).SelectedTabPageIndex = pageIndex;
    }
    public class TabControlObserver:ITabControlObserver{
        public IObservable<ITabControlProvider> WhenTabControl(DetailView detailView, IModelViewLayoutElement element) 
            => ((WinLayoutManager)detailView.LayoutManager).WhenItemCreated().Where(t => t.model == element).Select(t => t.control).Take(1)
                .SelectMany(tabbedControlGroup => detailView.LayoutManager.WhenLayoutCreated().Take(1).To(tabbedControlGroup))
                .Select(o => new TabControlProvider((TabbedControlGroup)o));
    }



}