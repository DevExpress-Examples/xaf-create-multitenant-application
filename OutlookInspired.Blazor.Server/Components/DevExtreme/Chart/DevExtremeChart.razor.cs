using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme.Chart{
    public class Model:ComponentModelBase,IComponentContentHolder{
        
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(model => model.Create<DevExtremeChart>());
    }
    
    
}