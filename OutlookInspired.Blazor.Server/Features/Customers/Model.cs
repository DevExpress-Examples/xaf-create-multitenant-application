using OutlookInspired.Blazor.Server.Components;
using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Customers{
    public abstract class Model<TModel>:RootListViewComponentModel<Customer,TModel> where TModel:Model<TModel>{
        
    }
}