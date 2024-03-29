﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Blazor.Server.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components{
    public abstract class ComponentModelBase<TComponent>:ComponentModelBase,IComponentContentHolder where TComponent:Microsoft.AspNetCore.Components.ComponentBase{
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(@base => @base.Create<TComponent>());
    }

    public abstract class ComponentModelBase:DevExpress.ExpressApp.Blazor.Components.Models.ComponentModelBase,IComplexControl{
        protected XafApplication Application;
        protected IObjectSpace ObjectSpace;
        private bool _clientIsReady;

        [JsonIgnore]
        public Action Update { get; set; } 
        public virtual void Setup(IObjectSpace objectSpace, XafApplication application){
            Application = application;
            ObjectSpace = objectSpace;
        }

        public virtual void ShowMessage(JsonElement element){
            var text = element.EnumerateArray().Select(e => e.GetString()).StringJoin(", ");
            if (text.StartsWith("W")){
                Application.ShowViewStrategy.ShowMessage(text,InformationType.Warning,10000);
                Tracing.Tracer.LogWarning(text);
            }
            else{
                Application.ShowViewStrategy.ShowMessage(text,InformationType.Error,60000);
                Tracing.Tracer.LogError(text);
            }
        }
        
        public virtual void Refresh(){ }
        public event EventHandler ClientReady;
        
        protected ComponentModelBase() => ReadyReference = new JsInterop(OnClientReady).DotNetReference();

        public bool ClientIsReady => _clientIsReady;
        protected virtual void OnClientReady(){
            _clientIsReady = true;
            ClientReady?.Invoke(this, EventArgs.Empty);
            
        }

        public DotNetObjectReference<JsInterop> ReadyReference{ get;  } 

        private void OnClientReady(JsonElement obj) => OnClientReady();
    }
}