using DevExpress.ExpressApp.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OutlookInspired.Blazor.Server.Services;

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
    
    public abstract class MapModel<TComponent>:DevExtremeModel<TComponent>,IComponentContentHolder where TComponent:Microsoft.AspNetCore.Components.ComponentBase{
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(model => model.Create<TComponent>());
        public bool PrintMap{
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }
    }

}