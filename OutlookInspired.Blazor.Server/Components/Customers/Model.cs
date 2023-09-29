using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Components.Customers{
    public abstract class Model:UserControlComponentModel{
        public List<Customer> Customers{
            get => GetPropertyValue<List<Customer>>();
            set => SetPropertyValue(value);
        }
        
        public override void Refresh() => Customers = ObjectSpace.GetObjects<Customer>(Criteria).ToList();

        public override RenderFragment ComponentContent => this.Create(FragmentSelector);
        protected abstract RenderFragment FragmentSelector(Model model);
        public override Type ObjectType => typeof(Customer);
        
    }
}