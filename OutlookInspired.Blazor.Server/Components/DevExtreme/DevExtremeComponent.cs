using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OutlookInspired.Blazor.Server.Components.DevExtreme.Maps;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme{
    public abstract class DevExtremeComponent:ComponentBase{
        protected IJSObjectReference ModuleRef;
        private  const string ResourceName = "DevExtremeComponent.js";
        static DevExtremeComponent(){
            using var memoryStream = new MemoryStream(Resource.Bytes());
            CreateResource<DevExtremeComponent>(memoryStream);
        }

        protected static void ExtractResources(string resourceName) 
            => typeof(DevExtremeMap).Assembly.GetManifestResourceStream(name => name.EndsWith(resourceName))
                .SaveToFile($"wwwroot/js/DevExtreme/{resourceName}");
        
        protected static void ExtractResources<T>() 
            => CreateResource<T>(typeof(DevExtremeMap).Assembly.GetManifestResourceStream(name => name.EndsWith($"{typeof(T)}.razor.js")));

        private static void CreateResource<T>(Stream manifestResourceStream) 
            => manifestResourceStream.SaveToFile($"wwwroot/js/DevExtreme/{DefaultResourceName(typeof(T))}");

        [Inject]
        public IJSRuntime JS{ get; set; }
        protected override async Task OnInitializedAsync(){
            ModuleRef = await ImportResource(ResourceName);
            await ModuleRef.InvokeVoidAsync("ensureDevExtremeAsync");
        }

        protected ValueTask<IJSObjectReference> ImportResource(string resourceName=null) 
            => JS.InvokeAsync<IJSObjectReference>("import", $"/js/DevExtreme/{GetResourceName(resourceName)}");

        private string GetResourceName(string resourceName) 
            => resourceName ?? DefaultResourceName(GetType());

        private static string DefaultResourceName(Type type) 
            => $"{type.Name}.js";

        private const string Resource = @"
let devExtremeInitPromise = null;

export async function ensureDevExtremeAsync() {
    await loadDevExtreme();
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