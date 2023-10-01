using Microsoft.AspNetCore.Components;

namespace OutlookInspired.Blazor.Server.Features.Customers.GridView{
    public class Model:Model<Model>{
        protected override RenderFragment FragmentSelector(Model model) => GridView.Create(model);
    }
}