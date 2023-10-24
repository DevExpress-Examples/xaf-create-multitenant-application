using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Components.Models{
    public abstract class UserControlComponentModel<T>:UserControlComponentModel{
        public List<T> Objects{
            get => GetPropertyValue<List<T>>();
            set => SetPropertyValue(value);
        }
        public override void Refresh() => Objects = ObjectSpace.GetObjects<T>(Criteria).ToList();
        public override Type ObjectType => typeof(T);
    }
    public abstract class RootListViewComponentModel<T,TModel,TComponent>:UserControlComponentModel<T> where TModel:ComponentModelBase where TComponent:ComponentBase{
        
        public override RenderFragment ComponentContent => ComponentModel.Create(model => model.Create<TComponent>());

        public virtual TModel ComponentModel => (TModel)(object)this;
        
        
    }
}