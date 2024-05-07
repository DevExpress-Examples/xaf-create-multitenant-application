using DevExpress.ExpressApp.Blazor;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme.Maps{
    public abstract class MapModel<TComponent>:DevExtremeModel<TComponent>,IComponentContentHolder where TComponent:Microsoft.AspNetCore.Components.ComponentBase{
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(model => model.Create<TComponent>());
        public bool PrintMap{
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }
    }
}