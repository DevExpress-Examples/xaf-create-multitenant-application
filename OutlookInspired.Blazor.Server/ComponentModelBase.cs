using System.Text.Json;
using Microsoft.JSInterop;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server{
    public abstract class ComponentModelBase:DevExpress.ExpressApp.Blazor.Components.Models.ComponentModelBase{
        public event EventHandler Ready;

        protected ComponentModelBase() => ReadyReference = new JsInterop(OnReady).DotNetReference();

        protected virtual void OnReady() => Ready?.Invoke(this, EventArgs.Empty);

        public DotNetObjectReference<JsInterop> ReadyReference{ get;  } 

        private void OnReady(JsonElement obj) => OnReady();
    }
}