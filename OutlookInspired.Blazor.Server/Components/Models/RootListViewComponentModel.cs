using System.Collections;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Components.Models{
    
    public abstract class RootListViewComponentModel<T,TModel,TComponent>:UserControlComponentModel<T> where TModel:ComponentModelBase where TComponent:ComponentBase{
        
        public override RenderFragment ComponentContent => ComponentModel.Create(model => model.Create<TComponent>());

        public virtual TModel ComponentModel => (TModel)(object)this;
        
        
    }
}