using System.Text.Json;
using Aqua.EnumerableExtensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme{
    public abstract class DevExtremeModel<TComponent>:ComponentModelBase<TComponent> where TComponent:ComponentBase{
        public override void ShowMessage(JsonElement element){
            if (!element.EnumerateArray().Select(e => e.GetString()).StringJoin(", ").Contains("js.devexpress.com")) return;
            base.ShowMessage(element);
        }
    }

    public abstract class DevExtremeComponent<TModel,TComponent>:ComponentBase<TModel,TComponent> where TModel:DevExtremeModel<TComponent>
        where TComponent:DevExtremeComponent<TModel,TComponent>{
        
        
        private  const string WwwRootPath = $"wwwroot/{JsPath}";
        private  const string JsPath = $"js/{ComponentName}";
        private  const string ComponentName = $"DevExtremeComponent";
        
        static DevExtremeComponent(){
            using var memoryStream = new MemoryStream(Script.Bytes());
            memoryStream.SaveToFile($"{WwwRootPath}/{ComponentName}.js");
        }
        
        protected override async Task OnInitializedAsync(){
            await JS.Console(ComponentModel.ShowMessage);
            await base.OnInitializedAsync();
            var devExtremeModule = await ImportResource(JsPath,$"{ComponentName}.js");
            await devExtremeModule.InvokeVoidAsync("ensureDevExtremeAsync");
            
        }
        
        private const string Script = $@"

let devExtremeInitPromise = null;
export async function ensureDevExtremeAsync() {{
    const scriptLoader = await import(`{ComponentBasePath}`);
    await loadDevExtreme(scriptLoader);
}}

function loadDevExtreme(scriptLoader) {{
    return devExtremeInitPromise || (devExtremeInitPromise = new Promise(async (resolve, _) => {{
        await scriptLoader.loadScriptAsync(""https://cdnjs.cloudflare.com/ajax/libs/devextreme-quill/1.6.2/dx-quill.min.js"");
        await scriptLoader.loadScriptAsync(""https://cdn3.devexpress.com/jslib/23.1.3/js/dx.all.js"");
        await scriptLoader.loadScriptAsync(""https://cdn3.devexpress.com/jslib/23.1.5/js/vectormap-data/usa.js"");
        await scriptLoader.loadScriptAsync(""https://cdn3.devexpress.com/jslib/23.1.5/js/vectormap-data/world.js"");
        await scriptLoader.loadStylesheetAsync(""https://cdn3.devexpress.com/jslib/23.1.3/css/dx.common.css"");
        await scriptLoader.loadStylesheetAsync(""https://cdn3.devexpress.com/jslib/23.1.3/css/dx.material.orange.dark.compact.css"");
        resolve();
    }}));
}}

";
    }
}