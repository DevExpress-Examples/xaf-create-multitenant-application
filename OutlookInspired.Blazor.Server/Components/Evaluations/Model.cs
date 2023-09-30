using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Blazor.Server.Components.Evaluations{
    public class Model:UserControlComponentModel{
        public Model() => Evaluations = new();

        public override RenderFragment ComponentContent => this.Create(GridView.Create);
        public List<Evaluation> Evaluations{
            get => GetPropertyValue<List<Evaluation>>();
            set => SetPropertyValue(value);
        }
        public override Type ObjectType => typeof(Evaluation);
        public override void Refresh(object currentObject) => Evaluations = ((Employee)currentObject).Evaluations.ToList();
    }
}