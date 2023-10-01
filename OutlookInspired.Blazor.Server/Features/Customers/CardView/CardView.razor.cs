using Microsoft.AspNetCore.Components;

namespace OutlookInspired.Blazor.Server.Features.Customers.CardView{
    public class Model:Model<Model>{
        protected override RenderFragment FragmentSelector(Model model) => CardView.Create(model);
    }
}