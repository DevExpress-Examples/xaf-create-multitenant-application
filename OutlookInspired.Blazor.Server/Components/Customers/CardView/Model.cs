using Microsoft.AspNetCore.Components;

namespace OutlookInspired.Blazor.Server.Components.Customers.CardView{
    public class Model:OutlookInspired.Blazor.Server.Components.Customers.Model{
        protected override RenderFragment FragmentSelector(Customers.Model model) => CardView.Create((Model)model);
    }
}