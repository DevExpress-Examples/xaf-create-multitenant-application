using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;

namespace OutlookInspired.Blazor.Server.Services{
    public static class Extensions{
        public static RenderFragment Create<T>(this T componentModel,Func<T,RenderFragment> fragmentSelector) where T:IComponentModel 
            => ComponentModelObserver.Create(componentModel, fragmentSelector(componentModel));
    }
}