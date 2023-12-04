using DevExpress.Blazor.Popup.Internal;
using Microsoft.JSInterop;
using OutlookInspired.Blazor.Server.Services.Internal;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme{
    public abstract class DevExtremeComponent<TModel,TComponent>:ComponentBase<TModel,TComponent> where TModel:DevExtremeModel<TComponent>
        where TComponent:DevExtremeComponent<TModel,TComponent>{
        private  const string ComponentName = "DevExtremeComponent";
        
        protected override async Task OneTimeInitializeAsync(){
            await base.OneTimeInitializeAsync();
            if (!InitializedTypes.Contains(typeof(DevExtremeComponent<,>))){
                InitializedTypes.Add(typeof(DevExtremeComponent<,>));
                using var memoryStream = new MemoryStream(_script.Bytes());
                await memoryStream.SaveToFileAsync($"{WwwRootPath}/{JsPath}/{ComponentName}.js");    
            }
        }

        protected override async Task OnAfterImportClientModuleAsync(bool firstRender){
            if (firstRender){
                var devExtremeModule = await ImportResource($"{ComponentName}.js");
                await devExtremeModule.InvokeVoidAsync("ensureDevExtremeAsync");
                DevExtremeModule = devExtremeModule;
                await OnAfterImportDevExtremeModuleAsync(true);
            }
            else{
                if (!(DevExtremeModule?.IsDisposed() ?? false)&&!(ClientModule?.IsDisposed() ?? false)){
                    await OnAfterImportDevExtremeModuleAsync(false);    
                }    
            }
        }

        protected virtual Task OnAfterImportDevExtremeModuleAsync(bool firstRender) => Task.CompletedTask;

        protected override async Task OnInitializedAsync(){
            await base.OnInitializedAsync();
            await JS.Console(ComponentModel.ShowMessage);
        }
        
        public IJSObjectReference DevExtremeModule { get; set; }

        private readonly string _script = $@"

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