using System.Linq.Expressions;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Components {
    public class ChartModel<T,TComponent>:ComponentModelBase,IComponentContentHolder where TComponent:ComponentBase{
        public IEnumerable<T> Data{
            get => GetPropertyValue<IEnumerable<T>>();
            set => SetPropertyValue(value);
        }

        public Expression<Func<T,string>> ArgumentField{ get; set; }
        public Expression<Func<T,decimal>> ValueField{ get; set; }
        public Expression<Func<T,string>> NameField{ get; set; }
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(model => model.Create<TComponent>());
    }
}
