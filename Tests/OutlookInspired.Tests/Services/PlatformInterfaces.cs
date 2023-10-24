using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace OutlookInspired.Tests.Assert{
    public interface IFilterViewAssertion{
        IObservable<Frame> Assert(IObservable<SingleChoiceAction> source);
    }
}