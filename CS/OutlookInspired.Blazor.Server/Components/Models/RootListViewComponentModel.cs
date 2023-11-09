using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Blazor.Server.Services.Internal;

namespace OutlookInspired.Blazor.Server.Components.Models{
    public abstract class RootListViewComponentModel<T,TModel,TComponent>:UserControlComponentModel<T> where TModel:ComponentModelBase where TComponent:Microsoft.AspNetCore.Components.ComponentBase{
        public override RenderFragment ComponentContent => ComponentModel.Create(model => model.Create<TComponent>());
        public virtual TModel ComponentModel => (TModel)(object)this;
    }
}