using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Services{
    public static class Extensions{
        public static void Render(this RenderTreeBuilder builder,string dataItemName,object propertyValue) 
            => builder.AddMarkupContent(0,   $@"
<div class=""dxbs-fl-ctrl""><!--!-->
    <div data-item-name=""{dataItemName}"" class=""d-none""></div><!--!-->
    <!--!-->{$"{propertyValue}".StringFormat()}<!--!-->
</div>
");

        public static RenderFragment Create<T>(this T componentModel,Func<T,RenderFragment> fragmentSelector) where T:IComponentModel 
            => ComponentModelObserver.Create(componentModel, fragmentSelector(componentModel));
    }
}