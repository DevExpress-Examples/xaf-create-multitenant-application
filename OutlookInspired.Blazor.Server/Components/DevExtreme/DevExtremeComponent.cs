using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme{
    public abstract class DevExtremeComponent:ComponentBase,IAsyncDisposable{
        protected IJSObjectReference ModuleRef;
        private bool _clientModuleInit;
        private  const string ResourceName = $"{nameof(DevExtremeComponent)}.js";
        static DevExtremeComponent(){
            using var memoryStream = new MemoryStream(Resource.Bytes());
            CreateResource<DevExtremeComponent>(memoryStream);
        }

        protected ElementReference Element { get; set; }
        protected IJSObjectReference ClientModule { get; set; }
        protected IJSObjectReference ClientObject { get; set; }
        protected IJSObjectReference DevExtremeModule{ get; set; }
        protected static void ExtractResource(string resourceName) 
            => typeof(DxMap).Assembly.GetManifestResourceStream(name => name.EndsWith(resourceName))
                .SaveToFile($"wwwroot/js/{nameof(DevExtremeComponent)}/{resourceName}");
        
        protected static void ExtractResource<T>() 
            => CreateResource<T>(typeof(DxMap).Assembly.GetManifestResourceStream(name => name.EndsWith($"{typeof(T)}.razor.js")));

        private static void CreateResource<T>(Stream manifestResourceStream) 
            => manifestResourceStream.SaveToFile($"wwwroot/js/{nameof(DevExtremeComponent)}/{DefaultResourceName(typeof(T))}");

        [Inject]
        public IJSRuntime JS{ get; set; }
        protected override async Task OnInitializedAsync(){
            DevExtremeModule = await ImportResource(ResourceName);
            await DevExtremeModule.InvokeVoidAsync("ensureDevExtremeAsync");
            ClientModule = await ImportResource();
        }

        

        protected override async Task OnAfterRenderAsync(bool firstRender){
            await base.OnAfterRenderAsync(firstRender);
            if (ClientModule != null&&!_clientModuleInit){
                _clientModuleInit = true;
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
            => JS.InvokeAsync<IJSObjectReference>("import", $"/js/{nameof(DevExtremeComponent)}/{GetResourceName(resourceName)}");

        private string GetResourceName(string resourceName) 
            => resourceName ?? DefaultResourceName(GetType());

        private static string DefaultResourceName(Type type) 
            => $"{type.Name}.js";

        private const string Resource = @"
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