using DevExpress.ExpressApp.Blazor.Components.Models;
using System.Linq.Expressions;

namespace OutlookInspired.Blazor.Server.ComponentModels {
    public class ChartModel<T, TArgument, TValue, TName> : ComponentModelBase {
        public IEnumerable<T> Data {
            get => GetPropertyValue<IEnumerable<T>>();
            set => SetPropertyValue(value);
        }
        public Expression<Func<T, TArgument>> ArgumentField {
            get => GetPropertyValue<Expression<Func<T, TArgument>>>();
            set => SetPropertyValue(value);
        }
        public Expression<Func<T, TValue>> ValueField {
            get => GetPropertyValue<Expression<Func<T, TValue>>>();
            set => SetPropertyValue(value);
        }
        public Expression<Func<T, TName>> NameField {
            get => GetPropertyValue<Expression<Func<T, TName>>>();
            set => SetPropertyValue(value);
        }
        public string Height {
            get => GetPropertyValue<string>("70vh");
            set => SetPropertyValue(value);
        }
        public override Type ComponentType => typeof(Components.XafChart<T, TArgument, TValue, TName>);
    }
}
