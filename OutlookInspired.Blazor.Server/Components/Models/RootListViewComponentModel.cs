using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Components.Models{
    public abstract class RootListViewComponentModel<T,TModel,TComponent>:UserControlComponentModel where T:BaseObject where TModel:RootListViewComponentModel<T,TModel,TComponent> where TComponent:ComponentBase{
        public List<T> Objects{
            get => GetPropertyValue<List<T>>();
            set => SetPropertyValue(value);
        }
        public override RenderFragment ComponentContent => ((TModel)this).Create(model => model.Create<TComponent>());

        public override void Refresh() => Objects = ObjectSpace.GetObjects<T>(Criteria).ToList();
        
        public override Type ObjectType => typeof(T);
    }
}