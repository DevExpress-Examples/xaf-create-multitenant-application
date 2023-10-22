using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.Assert;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Win.Tests.Common{
    public class FilterViewAssertion:IFilterViewAssertion{
        public IObservable<Frame> Assert(IObservable<SingleChoiceAction> source) 
            => source.AssertDialogControllerListView(typeof(ViewFilter), _ => AssertAction.DetailViewSave, true)
                .ToSecond().IgnoreElements();
    }
}