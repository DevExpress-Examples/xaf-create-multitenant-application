using DevExpress.ExpressApp.Actions;
using DevExpress.Map.Kml.Model;
using Unit = System.Reactive.Unit;

namespace OutlookInspired.Tests.Services{
    public interface IFilterViewAssertion{
        IObservable<Unit> AssertCreateNew(SingleChoiceAction action);
    }
}