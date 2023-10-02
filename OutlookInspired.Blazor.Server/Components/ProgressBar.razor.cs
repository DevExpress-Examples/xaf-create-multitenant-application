using DevExpress.ExpressApp.Blazor.Components.Models;

namespace OutlookInspired.Blazor.Server.Components{
    public class ProgressBarModel : ComponentModelBase{
        public string Width{
            get => GetPropertyValue<string>();
            set => SetPropertyValue( value);
        }
    }
}