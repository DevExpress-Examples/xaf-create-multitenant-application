using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Products{
    public abstract class Model<TModel>:RootListViewComponentModel<Product,TModel> where TModel:Model<TModel>{
        
    }
}