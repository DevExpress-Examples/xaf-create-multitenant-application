using System.Reactive.Linq;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Services;
using XAF.Testing;
using XAF.Testing.XAF;
using Unit = System.Reactive.Unit;

namespace OutlookInspired.Win.Tests.Common{
    class FilterViewManager:IFilterViewManager{
        public bool InlineEdit => true;
    }

    public class AssertFilterView:IAssertFilterView{
        public IObservable<Unit> AssertCreateNew(SingleChoiceAction action) 
            => action.AssertDialogControllerListView(typeof(ViewFilter), _ => AssertAction.DetailViewSave, true)
                .IgnoreElements().ToUnit();
    }
}