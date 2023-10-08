using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme{
    public class DevExtremeModel<TComponent>:ComponentModelBase,IComponentContentHolder where TComponent:ComponentBase{
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(@base => @base.Create<TComponent>());
    }
    
    public abstract class DevExtremeComponent<TModel,TComponent>:ComponentBase,IAsyncDisposable where TModel:DevExtremeModel<TComponent>
        where TComponent:DevExtremeComponent<TModel,TComponent>{
        private bool _clientModuleInit;
        
        private  const string WwwRootPath = $"wwwroot/{JsPath}";
        private  const string JsPath = $"js/{ComponentName}";
        private  const string ComponentName = $"DevExtremeComponent";
        // private  const string ResourceName = $"DevExtremeComponent.js";
        static DevExtremeComponent(){
            using var memoryStream = new MemoryStream(Script.Bytes());
            memoryStream.SaveToFile($"{WwwRootPath}/{ComponentName}.js");
        }
        
        [Parameter]
        public TModel ComponentModel { get; set; }

        protected ElementReference Element { get; set; }
        protected IJSObjectReference ClientModule { get; set; }
        protected IJSObjectReference ClientObject { get; set; }
        protected IJSObjectReference DevExtremeModule{ get; set; }
        protected static void ExtractResource(string resourceName) 
            => typeof(DevExtremeComponent<TModel,TComponent>).Assembly.GetManifestResourceStream(name => name.EndsWith(resourceName))
                .SaveToFile($"{WwwRootPath}/{resourceName}");
        [Parameter]
        public EventCallback<TModel> CustomizeModel { get; set; }
        protected static void ExtractResource<T>() 
            => CreateResource<T>(typeof(DevExtremeComponent<TModel,TComponent>).Assembly.GetManifestResourceStream(name => name.EndsWith($"{typeof(T)}.razor.js")));

        private static void CreateResource<T>(Stream manifestResourceStream) 
            => manifestResourceStream.SaveToFile($"{WwwRootPath}//{DefaultResourceName(typeof(T))}");
        

        [Inject]
        public IJSRuntime JS{ get; set; }
        protected override async Task OnInitializedAsync(){
            DevExtremeModule = await ImportResource($"{ComponentName}.js");
            await DevExtremeModule.InvokeVoidAsync("ensureDevExtremeAsync");
            ClientModule = await ImportResource();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender){
            await base.OnAfterRenderAsync(firstRender);
            if (ClientModule != null&&!_clientModuleInit){
                _clientModuleInit = true;
                await CustomizeModel.MaybeInvokeAsync(ComponentModel);
                await OnAfterRenderClientModuleAsync();
                _clientModuleInit = false;
            }
        }

        protected abstract Task OnAfterRenderClientModuleAsync();

        async ValueTask IAsyncDisposable.DisposeAsync() {
            if(ClientObject != null)
                await ClientObject.DisposeAsync();
            if(ClientModule != null)
                await ClientModule.DisposeAsync();
        }

        protected ValueTask<IJSObjectReference> ImportResource(string resourceName=null) 
            => JS.InvokeAsync<IJSObjectReference>("import", $"/{JsPath}/{GetResourceName(resourceName)}");

        private string GetResourceName(string resourceName) 
            => resourceName ?? DefaultResourceName(GetType());

        private static string DefaultResourceName(Type type) 
            => $"{type.Name}.js";

        private const string Script = @"
let devExtremeInitPromise = null;

export async function ensureDevExtremeAsync() {
    await loadDevExtreme();
}
export function printElement(element) {
    document.body.innerHTML = """";
    document.body.appendChild(element);
    setTimeout(() => {
        window.print();
        location.reload();
    }, 2000);
}
function loadDevExtreme() {
    return devExtremeInitPromise || (devExtremeInitPromise = new Promise(async (resolve, _) => {
        await loadScriptAsync(""https://cdnjs.cloudflare.com/ajax/libs/devextreme-quill/1.6.2/dx-quill.min.js"");
        await loadScriptAsync(""https://cdn3.devexpress.com/jslib/23.1.3/js/dx.all.js"");
        await loadScriptAsync(""https://cdn3.devexpress.com/jslib/23.1.5/js/vectormap-data/usa.js"");
        await loadScriptAsync(""https://cdn3.devexpress.com/jslib/23.1.5/js/vectormap-data/world.js"");
        await loadStylesheetAsync(""https://cdn3.devexpress.com/jslib/23.1.3/css/dx.common.css"");
        await loadStylesheetAsync(""https://cdn3.devexpress.com/jslib/23.1.3/css/dx.material.orange.dark.compact.css"");
        resolve();
    }));

    function loadScriptAsync(src) {
        return new Promise((resolve, _) => {
            const scriptEl = document.createElement(""SCRIPT"");
            scriptEl.src = src;
            scriptEl.onload = resolve;
            document.head.appendChild(scriptEl);
        });
    }
    function loadStylesheetAsync(href) {
        return new Promise((resolve, _) => {
            const stylesheetEl = document.createElement(""LINK"");
            stylesheetEl.href = href;
            stylesheetEl.rel = ""stylesheet"";
            stylesheetEl.onload = resolve;
            document.head.appendChild(stylesheetEl);
        });
    }
}

";
    }
}