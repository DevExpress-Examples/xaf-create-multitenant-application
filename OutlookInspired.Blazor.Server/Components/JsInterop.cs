using Microsoft.JSInterop;

namespace OutlookInspired.Blazor.Server.Components{
    public class JsInterop {
        private readonly Action<object> _action;
        public JsInterop(Action<object> action) => _action = action;

        [JSInvokable]
        public void Invoke(object param) => _action.Invoke(param);
    }
}