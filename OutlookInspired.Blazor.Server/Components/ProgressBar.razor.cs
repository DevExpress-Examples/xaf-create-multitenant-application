using DevExpress.ExpressApp.Blazor.Components.Models;

namespace OutlookInspired.Blazor.Server.Components{
    public class ProgressBarModel : ComponentModelBase{
        public int Width{
            get => GetPropertyValue<int>();
            set => SetPropertyValue( value);
        }

        
    }
}