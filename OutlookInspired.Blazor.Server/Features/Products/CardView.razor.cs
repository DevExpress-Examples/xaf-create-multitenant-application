using Microsoft.AspNetCore.Components;

namespace OutlookInspired.Blazor.Server.Features.Products{
    public class Model:Model<Model>{
        protected override RenderFragment FragmentSelector(Model model) => CardView.Create(model);
    }
}