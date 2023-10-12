using System.Reactive;
using DevExpress.ExpressApp;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace XAF.Testing.Win.XAF{
    public class ReportAsserter : IReportAsserter{
        public IObservable<Unit> AssertReport(Frame frame, string item) 
            => frame.AssertReport(item).ToUnit();
    }
}