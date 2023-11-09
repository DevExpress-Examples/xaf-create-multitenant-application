using Microsoft.JSInterop;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme.Maps{
    public abstract class MapComponent<TModel,TComponent>:DevExtremeComponent<TModel,TComponent> where TModel : MapModel<TComponent> where TComponent : DevExtremeComponent<TModel, TComponent>{
        protected override async Task OnAfterImportClientModuleAsync(bool firstRender){
            await base.OnAfterImportClientModuleAsync(firstRender);
            var printMap = ComponentModel.PrintMap;
            if (printMap){
                ComponentModel.PrintMap = false;
                await ScriptLoader.InvokeVoidAsync("printElement", Element);
            }
        }
    }
}