using System.Reactive.Linq;
using DevExpress.ExpressApp;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace XAF.Testing.Win.XAF{
    public class NewObjectController : INewObjectController{
        public IObservable<Frame> CreateNewObjectController(Frame frame) 
            => frame.View.WhenObjectViewObjects(1).Take(1)
                .SelectMany(selectedObject => frame.ColumnViewCreateNewObject()
                    .SwitchIfEmpty(frame.ListViewCreateNewObject())
                    .SelectMany(newObjectFrame => newObjectFrame.View.ToCompositeView()
                        .CloneExistingObjectMembers(false, selectedObject)
                        .Select(_ => default(Frame)).IgnoreElements().Concat(newObjectFrame.YieldItem())));
    }
}