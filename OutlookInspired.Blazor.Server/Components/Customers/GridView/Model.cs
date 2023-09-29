using Microsoft.AspNetCore.Components;

namespace OutlookInspired.Blazor.Server.Components.Customers.GridView{
    public class Model:OutlookInspired.Blazor.Server.Components.Customers.Model{
        protected override RenderFragment FragmentSelector(Customers.Model model) => GridView.Create((Model)model);
    }
}