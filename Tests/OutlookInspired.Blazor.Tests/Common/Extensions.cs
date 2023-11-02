using System.Reactive;
using System.Reactive.Linq;
using OutlookInspired.Blazor.Server.Components;
using XAF.Testing.RX;

namespace OutlookInspired.Blazor.Tests.Common{
    static class Extensions{
        public static IObservable<Unit> WhenClientIsReady(this ComponentModelBase model) 
            => model.WhenEvent(nameof(ComponentModelBase.ClientReady))
                .Select(_ => model.ClientIsReady).StartWith(model.ClientIsReady)
                .WhenNotDefault().ToUnit();
    }
}