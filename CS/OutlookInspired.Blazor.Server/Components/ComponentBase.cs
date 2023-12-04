using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components{
    public abstract class ComponentBase:Microsoft.AspNetCore.Components.ComponentBase{
        public static readonly string JsPath = "js";
        protected static readonly string ComponentBasePath = $"/{JsPath}/{ComponentBaseName}.js";
        protected static string WwwRootPath=null;
        protected const string ComponentBaseName = "ComponentBase";
        private static readonly SemaphoreSlim Semaphore = new(1, 1);
        protected static readonly HashSet<Type> InitializedTypes = new();
        [Inject]
        public IWebHostEnvironment WebHostEnvironment{ get; set; }

        protected override async Task OnInitializedAsync(){
            await Semaphore.WaitAsync();
            try{
                var type = GetType();
                if (InitializedTypes.Contains(type)) return;
                InitializedTypes.Add(type);
                await OneTimeInitializeAsync();
            }
            finally{
                Semaphore.Release();
            }
        }
        
        protected virtual async Task OneTimeInitializeAsync(){
            if (!InitializedTypes.Contains(typeof(ComponentBase))){
                WwwRootPath = WebHostEnvironment.ContentRootPath;
                using var memoryStream = new MemoryStream(Script.Bytes());
                await memoryStream.SaveToFileAsync($"{WwwRootPath}/{JsPath}/{ComponentBaseName}.js");
                InitializedTypes.Add(typeof(ComponentBase));    
            }
            await ExtractResourceAsync();
        }
        
        protected async Task ExtractResourceAsync() 
            => await CreateResourceAsync(GetType().Assembly.GetManifestResourceStream(name => name.EndsWith($"{GetType()}.razor.js")));
        
        private async Task CreateResourceAsync(Stream manifestResourceStream){
            var filePath = $"{WwwRootPath}/{JsPath}/{DefaultResourceName(GetType())}";
            await manifestResourceStream.SaveToFileAsync(filePath);
        }

        protected static string DefaultResourceName(Type type) 
            => $"{type.Name}.js";

        private const string Script = @"
    export function printElement(element){
        document.body.innerHTML = '';
        document.body.appendChild(element);
        setTimeout(() => {
            window.print();
            location.reload();
        }, 2000);
    };

    export function loadScriptAsync(src) {
        return new Promise((resolve, _) => {
            const scriptEl = document.createElement(""SCRIPT"");
            scriptEl.src = src;
            scriptEl.onload = resolve;
            document.head.appendChild(scriptEl);
        });
    };

    export function loadStylesheetAsync(href){
        return new Promise((resolve, _) => {
            const stylesheetEl = document.createElement(""LINK"");
            stylesheetEl.href = href;
            stylesheetEl.rel = ""stylesheet"";
            stylesheetEl.onload = resolve;
            document.head.appendChild(stylesheetEl);
        });
    };
";

    }
    public abstract class ComponentBase<TModel,TComponent>:ComponentBase,IAsyncDisposable where TModel:ComponentModelBase<TComponent>
        where TComponent:Microsoft.AspNetCore.Components.ComponentBase{
        
        protected virtual Task OnAfterImportClientModuleAsync(bool firstRender) => Task.CompletedTask;
        
        async ValueTask IAsyncDisposable.DisposeAsync(){
            try{
                if(ClientObject != null)
                    await ClientObject.DisposeAsync();
                if(ClientModule != null)
                    await ClientModule.DisposeAsync();
            }
            catch (JSDisconnectedException){
            }
        }
        
        [Parameter]
        public TModel ComponentModel { get; set; }

        public ElementReference Element { get; set; }
        public IJSObjectReference ClientModule { get; set; }
        protected IJSObjectReference ClientObject { get; set; }
        
        [Inject]
        public IJSRuntime JS{ get; set; }

        
        protected override async Task OnInitializedAsync(){
            await base.OnInitializedAsync();
            ComponentModel.Update = StateHasChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender){
            if (firstRender){
                ScriptLoader ??= await ImportResource($"{ComponentBaseName}.js");
                ClientModule ??= await ImportResource();
                await OnAfterImportClientModuleAsync(true);
            }
            else{
                if (ClientModule != null){
                    await OnAfterImportClientModuleAsync(false);
                    
                }
            }
        }

        protected ValueTask<IJSObjectReference> ImportResource(string resourceName=null) 
            => JS.InvokeAsync<IJSObjectReference>("import", $"/{JsPath}/{GetResourceName(resourceName)}");
        
        private string GetResourceName(string resourceName) 
            => resourceName ?? DefaultResourceName(GetType());
        
        protected IJSObjectReference ScriptLoader{ get; set; }
    }
}