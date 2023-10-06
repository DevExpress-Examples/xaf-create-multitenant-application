using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Customers{
    public abstract class Model<TModel,TComponent>:RootListViewComponentModel<Customer,TModel,TComponent> where TModel:Model<TModel,TComponent> where TComponent : ComponentBase{
        
    }
}