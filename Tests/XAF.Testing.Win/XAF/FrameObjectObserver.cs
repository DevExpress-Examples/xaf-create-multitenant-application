using System.Reactive.Linq;
using DevExpress.ExpressApp;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace XAF.Testing.Win.XAF{
    public class FrameObjectObserver : IFrameObjectObserver{
        IObservable<(Frame frame, object o)> IFrameObjectObserver.WhenObjects(Frame frame, int count ) 
            => frame.WhenColumnViewObjects(count).SwitchIfEmpty(Observable.Defer(() =>
                    frame.View.Observe().SelectMany(view => view.WhenObjectViewObjects(count))))
                .Select(obj => (frame, o: obj));
    }
}