using DevExpress.ExpressApp.Actions;
using Unit = System.Reactive.Unit;

namespace OutlookInspired.Tests.Services{
    public interface IAssertFilterView{
        IObservable<Unit> AssertCreateNew(SingleChoiceAction action);
    }

    public interface IFilterViewManager{
        bool InlineEdit{ get; }
    }
}