using System.Text.Json;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme.Maps{
    public class DxMap1Model : ComponentModelBase, IComponentContentHolder{
        public event EventHandler<MapItemSelectedArgs> MapItemSelected;
        public void SelectMapItem(JsonElement item) 
            => MapItemSelected?.Invoke(this, new MapItemSelectedArgs(item));
        public MapOptions Options{ get; set; } = new();
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(model => model.Create<DxVectorMap>());
        public bool PrintMap{
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }

        
    }
    
    public class MapOptions{
    }

    

    
}