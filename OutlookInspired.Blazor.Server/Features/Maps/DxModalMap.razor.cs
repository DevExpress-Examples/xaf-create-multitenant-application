using DevExpress.ExpressApp.Blazor;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components.DevExtreme;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public class DxModalMapModel:DxVectorMapModel,IComponentContentHolder{
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(model => model.Create<DxModalMap>());
    }
}