using DevExpress.ExpressApp.Blazor.Components.Models;

namespace OutlookInspired.Blazor.Server.ComponentModels {
    public class ProgressBarModel : ComponentModelBase {
        public int Width {
            get => GetPropertyValue<int>();
            set => SetPropertyValue(value);
        }
        public override Type ComponentType => typeof(Components.ProgressBar);
    }
}