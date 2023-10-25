using System.Text.Json;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server{
    public abstract class ComponentModelBase<TComponent>:ComponentModelBase,IComponentContentHolder where TComponent:ComponentBase{
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(@base => @base.Create<TComponent>());
    }

    public abstract class ComponentModelBase:DevExpress.ExpressApp.Blazor.Components.Models.ComponentModelBase,IComplexControl{
        private XafApplication _application;
        public void Setup(IObjectSpace objectSpace, XafApplication application) => _application=application;
        public virtual void ShowMessage(JsonElement element){
            var text = element.EnumerateArray().Select(e => e.GetString()).StringJoin(", ");
            if (text.StartsWith("W")){
                _application.ShowViewStrategy.ShowMessage(text,InformationType.Warning,10000);
                Tracing.Tracer.LogWarning(text);
            }
            else{
                _application.ShowViewStrategy.ShowMessage(text,InformationType.Error,60000);
                Tracing.Tracer.LogError(text);
            }
        }
        

        public void Refresh(){ }
        public event EventHandler Ready;

        protected ComponentModelBase() => ReadyReference = new JsInterop(OnReady).DotNetReference();

        protected virtual void OnReady() => Ready?.Invoke(this, EventArgs.Empty);

        public DotNetObjectReference<JsInterop> ReadyReference{ get;  } 

        private void OnReady(JsonElement obj) => OnReady();
    }
}