using System.Linq.Expressions;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Components {
    public class ChartModel<TObject,TArgument,TValue,TName,TComponent>:ComponentModelBase,IComponentContentHolder where TComponent:ComponentBase{
        public IEnumerable<TObject> Data{
            get => GetPropertyValue<IEnumerable<TObject>>();
            set => SetPropertyValue(value);
        }

        public Expression<Func<TObject,TArgument>> ArgumentField{ get; set; }
        public Expression<Func<TObject,TValue>> ValueField{ get; set; }
        public Expression<Func<TObject,TName>> NameField{ get; set; }
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(model => model.Create<TComponent>());
        public string Height{ get; set; } = "70vh";
        public Expression<Func<object,string>> NameField1{ get; set; }
    }
}
