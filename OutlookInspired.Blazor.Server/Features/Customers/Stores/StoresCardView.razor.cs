using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components.Models;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Blazor.Server.Services.Internal;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Features.Customers.Stores{
    public class Model:UserControlComponentModel{
        public Model() => Stores = new();

        public override RenderFragment ComponentContent => this.Create(StoresCardView.Create);
        public List<CustomerStore> Stores{
            get => GetPropertyValue<List<CustomerStore>>();
            set => SetPropertyValue(value);
        }
        public override Type ObjectType => typeof(CustomerStore);
        public override void Refresh(object currentObject) => Stores = ((Customer)currentObject)?.CustomerStores.ToList()??new List<CustomerStore>();
    }
}