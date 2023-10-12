using System.Reactive.Linq;
using DevExpress.ExpressApp.Win.Editors;
using XAF.Testing.RX;
using XAF.Testing.XAF;
using ListView = DevExpress.ExpressApp.ListView;

namespace XAF.Testing.Win.XAF{
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
}