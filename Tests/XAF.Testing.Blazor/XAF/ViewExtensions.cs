using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Layout;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Office.Blazor.Editors.Adapters;
using XAF.Testing.XAF;
using Observable = System.Reactive.Linq.Observable;

namespace XAF.Testing.Blazor.XAF{
    public static class ViewExtensions{

        public static void ClearFilter(this ListView listView) => ((DxGridListEditor)listView.Editor).GetGridAdapter().GridInstance.SetFilterCriteria(null);
        public static IObservable<object> AssertRichEditControl(this DetailView detailView)
            => detailView.WhenPropertyEditorControl().Cast<RichTextEditorComponentAdapter>().Select(adapter => adapter.ComponentModel)
                .AssertRichEditControl();

        public static IObservable<(IModelViewLayoutElement model,object control,ViewItem viewItem)> WhenItemCreated(this BlazorLayoutManager layoutManager) 
            => layoutManager.WhenEvent<BlazorLayoutManager.ItemCreatedEventArgs>(nameof(BlazorLayoutManager.ItemCreated))
                .Select(e => (e.ModelLayoutElement,(object)e.LayoutControlItem, e.ViewItem));

        public static IObservable<ITabControlProvider> WhenTabControl(this DetailView detailView, IModelViewLayoutElement element) 
            => ((BlazorLayoutManager)detailView.LayoutManager).WhenItemCreated().Where(t => t.model == element)
                .Select(t => new TabControlProvider((DxFormLayoutTabPagesModel)t.control, ((IModelTabbedGroup)t.model).Count));
        
        
        public static IObservable<T> SelectObject<T>(this ListView view, params T[] objects) where T : class{
            var viewEditor = (view.Editor as DxGridListEditor);
            if (viewEditor == null)
                throw new NotImplementedException(nameof(ListView.Editor));
            viewEditor.UnselectObjects(viewEditor.GetSelectedObjects());
            return objects.ToNowObservable()
                .SwitchIfEmpty(Observable.Defer(() => viewEditor.DataSource.ObserveItems(1).OfType<T>()))
                .Do(obj => viewEditor.SelectObject(obj));
        }
    }
}