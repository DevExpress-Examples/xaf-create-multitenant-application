using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components{
    public abstract class ComponentBase<TModel,TComponent>:ComponentBase,IAsyncDisposable where TModel:ComponentModelBase<TComponent>
        where TComponent:ComponentBase{
        private bool _clientModuleInit;
        protected const string ComponentBasePath=$"/{JsPath}/{ComponentBaseName}.js";
        private  const string WwwRootPath = $"wwwroot/{JsPath}";
        private  const string JsPath = $"js/{ComponentBaseName}";
        protected const string ComponentBaseName = "ComponentBase";
        static ComponentBase(){
            using var memoryStream = new MemoryStream(Script.Bytes());
            memoryStream.SaveToFile($"{WwwRootPath}/{ComponentBaseName}.js");
        }

        protected virtual Task OnAfterRenderClientModuleAsync() => Task.CompletedTask;

        protected override async Task OnAfterRenderAsync(bool firstRender){
            await base.OnAfterRenderAsync(firstRender);
            if (ClientModule != null&&!_clientModuleInit){
                _clientModuleInit = true;
                await CustomizeModel.MaybeInvokeAsync(ComponentModel);
                await OnAfterRenderClientModuleAsync();
                _clientModuleInit = false;
            }
        }
        protected static void ExtractResource<T>() 
            => CreateResource<T>(typeof(T).Assembly.GetManifestResourceStream(name => name.EndsWith($"{typeof(T)}.razor.js")));
        
        private static void CreateResource<T>(Stream manifestResourceStream) 
            => manifestResourceStream.SaveToFile($"{WwwRootPath}//{DefaultResourceName(typeof(T))}");

        protected static string DefaultResourceName(Type type) 
            => $"{type.Name}.js";

        async ValueTask IAsyncDisposable.DisposeAsync(){
            if(ClientObject != null)
                await ClientObject.DisposeAsync();
            if(ClientModule != null)
                await ClientModule.DisposeAsync();
        }
        
        [Parameter]
        public TModel ComponentModel { get; set; }

        protected ElementReference Element { get; set; }
        protected IJSObjectReference ClientModule { get; set; }
        protected IJSObjectReference ClientObject { get; set; }
        [Parameter]
        public EventCallback<TModel> CustomizeModel { get; set; }
        
        
        [Inject]
        public IJSRuntime JS{ get; set; }
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

        protected override async Task OnInitializedAsync(){
            ScriptLoader = await ImportResource(JsPath, $"{ComponentBaseName}.js");
            ClientModule = await ImportResource(JsPath);
        }

        protected ValueTask<IJSObjectReference> ImportResource(string jsPath,string resourceName=null) 
            => JS.InvokeAsync<IJSObjectReference>("import", $"/{jsPath}/{GetResourceName(resourceName)}");
        
        private string GetResourceName(string resourceName) 
            => resourceName ?? DefaultResourceName(GetType());
        
        protected IJSObjectReference ScriptLoader{ get; set; }
    }
}