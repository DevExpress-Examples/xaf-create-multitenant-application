using System.Reactive;
using System.Reactive.Linq;
using OutlookInspired.Blazor.Server;
using XAF.Testing.RX;

namespace OutlookInspired.Blazor.Tests.Common{
    static class Extensions{
        public static IObservable<Unit> WhenClientIsReady(this ComponentModelBase model){
            return model.WhenEvent(nameof(ComponentModelBase.ClientReady)).To(model.ClientIsReady).StartWith(model.ClientIsReady).WhenNotDefault().ToUnit();
        }
    }
}