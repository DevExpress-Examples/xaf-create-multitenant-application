using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.DC;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Services{
    internal static class Extensions{
        public static async Task<string> ModalBodyHeight(this IJSRuntime js){
            await js.EvalAsync(@"window.getClientHeight = (element) => {
    return document.querySelector(element).closest('.dxbl-modal-body').clientHeight;
};");
            return $"{await js.InvokeAsync<double>("getClientHeight", ".dxbl-modal-body")}";
        }

        public static Task MaybeInvokeAsync<T>(this EventCallback<T> eventCallback, T value) 
            => eventCallback.HasDelegate ? eventCallback.InvokeAsync(value) : Task.CompletedTask;

        

        public static RenderFragment RenderIconCssOrImage(this IImageUrlService service, string imageName, string className = "xaf-image",bool useSvgIcon=false)
            => DxImage.IconCssOrImage(null, service.GetImageUrl(imageName), className,useSvgIcon);
        
        public static RenderFragment BootFragment(this Evaluation evaluation,Func<Evaluation,Enum> boost ) 
            => builder =>{
                builder.OpenComponent(2, typeof(XafImage));
                builder.AddAttribute(3, "ImageName", boost(evaluation).ImageInfo().ImageName);
                builder.AddAttribute(4, "Size", 16);
                builder.AddAttribute(5,"Color", Convert.ToInt32(boost(evaluation))==0?"red":"green");
                builder.CloseComponent();
            };

        public static async ValueTask AddGridColumnTextOverflow(this IJSRuntime runtime, bool firstRender, string classname) 
            => await runtime.EvalAsync(firstRender, $@"
                    let attempts = 0;
                    function findCells() {{
                        attempts++;
                        let cells = document.querySelectorAll('.{classname}');
                        if (cells.length > 0 || attempts >= 10) {{
                            clearInterval(intervalId);
                        }}
                        cells.forEach((cell, index) => {{
                            let parentTd = cell.closest('td');  
                            let parentTdWidth = parentTd ? parentTd.offsetWidth : '100%';  
                            cell.style.maxWidth = parentTdWidth + 'px';  
                            cell.style.whiteSpace = 'nowrap';
                            cell.style.overflow = 'hidden';
                            cell.style.textOverflow = 'ellipsis';
                            cell.style.visibility = 'visible';
                        }});
                    }}
                    let intervalId = setInterval(findCells, 100);");

        public static string FontSize(this GridDataColumnCellDisplayTemplateContext context) 
            => ((IObjectSpaceLink)context.DataItem).ObjectSpace.TypesInfo.FindTypeInfo(typeof(Evaluation))
            .FindMember(context.DataColumn.FieldName)
            .FontSize();

        public static string FontSize(this IMemberInfo info){
            var fontSizeDeltaAttribute = info.FindAttribute<FontSizeDeltaAttribute>();
            return fontSizeDeltaAttribute != null ? $"font-size: {(fontSizeDeltaAttribute.Delta == 8 ? "1.8" : "1.2")}rem" : null;
        }
        public static async ValueTask EvalAsync(this IJSRuntime runtime,bool firstRender,params object[] args){
            if (firstRender){
                await runtime.InvokeVoidAsync("eval", args);
            }
        }
        public static async ValueTask EvalAsync(this IJSRuntime runtime,params object[] args) 
            => await runtime.EvalAsync(true, args);
        public static async ValueTask EvalJSAsync(this XafApplication application,params object[] args) 
            => await application.ServiceProvider.GetRequiredService<IJSRuntime>().EvalAsync(true,args);
        public static DotNetObjectReference<T> DotNetReference<T>(this T value) where T:class 
            => DotNetObjectReference.Create(value);
        
        public static void RenderMarkup(this RenderTreeBuilder builder,string dataItemName,object value) 
            => builder.AddMarkupContent(0,   $@"
<div class=""dxbs-fl-ctrl""><!--!-->
    <div data-item-name=""{dataItemName}"" class=""d-none""></div><!--!-->
    <!--!-->{$"{value}".StringFormat()}<!--!-->
</div>
");

        public static RenderFragment Create<T>(this IComponentModel model)  
            => builder => {
                builder.OpenComponent(0, typeof(T));
                builder.AddAttribute(1, "ComponentModel", model);
                builder.CloseComponent();
            };
        
        public static RenderFragment Create<T>(this T componentModel,Func<T,RenderFragment> fragmentSelector) where T:IComponentModel 
            => ComponentModelObserver.Create(componentModel, fragmentSelector(componentModel));
        
    }
}