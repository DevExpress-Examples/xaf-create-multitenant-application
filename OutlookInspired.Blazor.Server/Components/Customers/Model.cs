using System.Collections;
using DevExpress.ExpressApp;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Components.Customers{
    public abstract class Model:UserControlComponentModel{
        protected Model() => SelectedCustomers = new();

        public List<Customer> Customers{
            get => GetPropertyValue<List<Customer>>();
            set => SetPropertyValue(value);
        }
        public List<Customer> SelectedCustomers{
            get => GetPropertyValue<List<Customer>>();
            set{
                SetPropertyValue(value);
                OnSelectionChanged();
            }
        }
        
        public override void Setup(IObjectSpace objectSpace, XafApplication application) 
            => Customers = objectSpace.GetObjectsQuery<Customer>().ToList();


        public override RenderFragment ComponentContent => this.Create(FragmentSelector);

        protected abstract RenderFragment FragmentSelector(Model model);

        public override IList SelectedObjects => SelectedCustomers;
        
        public override Type ObjectType => typeof(Customer);
        
        public void ShowDetailView() => OnProcessObject();
    }
}