using DevExpress.ExpressApp.Blazor.Components.Models;

namespace OutlookInspired.Blazor.Server.Components{
    public class HyperlinkModel:ComponentModelBase{
        

        public string Text{
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }
        public string Style{
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }
        public string Href{
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        
    }
}