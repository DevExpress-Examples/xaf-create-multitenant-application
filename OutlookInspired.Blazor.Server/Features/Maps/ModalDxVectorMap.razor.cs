using DevExpress.ExpressApp.Blazor;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Components.DevExtreme.Maps;
using OutlookInspired.Blazor.Server.Services;

namespace OutlookInspired.Blazor.Server.Features.Maps{
    public class ModalDxVectorMapModel:DxVectorMapModel,IComponentContentHolder{
        RenderFragment IComponentContentHolder.ComponentContent => this.Create(model => model.Create<ModalDxVectorMap>());
    }
}