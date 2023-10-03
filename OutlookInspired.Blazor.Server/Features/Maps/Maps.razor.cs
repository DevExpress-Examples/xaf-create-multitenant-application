using DevExpress.ExpressApp.Blazor;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public class Model:OutlookInspired.Blazor.Server.Components.DevExtreme.Model,IComponentContentHolder{
        public RenderFragment ComponentContent => this.Create(model => model.Create<Maps>());
        
    }

}