using System.Text.RegularExpressions;
using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.DC;
using DevExpress.Map.Native;
using DevExpress.Persistent.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using OutlookInspired.Blazor.Server.Components.DevExtreme;
using OutlookInspired.Module.Attributes;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Blazor.Server.Services{
    public static class Extensions{
        private static readonly Regex RemoveTagRegex = new(@"<[^>]*>", RegexOptions.Compiled);
        public static MapSettings MapSettings(this ISalesMapsMarker marker, Period period)
            => Components.DevExtreme.MapSettings.New(marker, period);

        public static MapItem[] Colorize(this MapItem[] mapItems, string[] palette,Type markerType) 
            => mapItems.GroupBy(item => item.PropertyValue(markerType))
                .SelectMany((items, i) => items.Do(item => item.Color = palette[i])).ToArray();

        public static object Features(this MapItem[] mapItems) 
            => new FeatureCollection{ Features = mapItems.GroupBy(item => item.City).Select(group => group.First().NewFeature(group)).ToList() };

        private static Feature NewFeature(this MapItem mapItem, IGrouping<string, MapItem> group) 
            => new(){
                Geometry = new Geometry{ Coordinates = new List<double>{ mapItem.Longitude, mapItem.Latitude } },
                Properties = new Properties{
                    Values = group.Select(item => item.Total).ToList(),
                    Tooltip = $"<span class='{mapItem.City}'>{mapItem.City} Total: {group.Sum(item => item.Total)}</span>",
                    City = mapItem.City
                }
            };

        public static async Task<RouteCalculatedArgs> ManeuverInstructions(this IObjectSpace objectSpace, Location locationA,Location locationB,string travelMode,string apiKey){
            var url = $"https://dev.virtualearth.net/REST/V1/Routes/{travelMode}?wp.0={locationA.Lat},{locationA.Lng}&wp.1={locationB.Lat},{locationB.Lng}&key={apiKey}";
            using var httpClient = new HttpClient();
            var result = JObject.Parse(await (await httpClient.GetAsync(url)).Content.ReadAsStringAsync())["resourceSets"]![0]!["resources"]![0];
            return new RouteCalculatedArgs(result!["routeLegs"]!.SelectMany(leg => leg["itineraryItems"])
                    .Select(objectSpace.RoutePoint).ToArray(), (double)result["travelDistance"],
                TimeSpan.FromMinutes((double)result["travelDuration"]), Enum.Parse<TravelMode>(travelMode,true));
        }

        private static RoutePoint RoutePoint(this IObjectSpace objectSpace, JToken item){
            var point = objectSpace.CreateObject<RoutePoint>();
            point.ManeuverInstruction = RemoveTagRegex.Replace(item["instruction"]["text"]!.ToString(), string.Empty);
            var distance = (double)item["travelDistance"];
            point.Distance = distance > 0.9 ? $"{Math.Ceiling(distance):0} mi" : $"{Math.Ceiling(distance * 52.8) * 100:0} ft";
            point.Maneuver = Enum.Parse<BingManeuverType>(item["details"][0]!["maneuverType"]!.ToString());
            return point;
        }
        
        public static MapSettings MapSettings(this IMapsMarker mapsMarker, IMapsMarker homeOffice, string travelMode){
            var mapSettings = new MapSettings();
            mapSettings.Markers.Add(new MapMarker()
                { Location = new Location(){ Lat = homeOffice.Latitude, Lng = homeOffice.Longitude } });
            mapSettings.Markers.Add(new MapMarker()
                { Location = new Location(){ Lat = mapsMarker.Latitude, Lng = mapsMarker.Longitude } });
            var mode = travelMode.FirstCharacterToLower();
            mapSettings.Routes = new List<MapRoute>()
                { new(){Mode =mode,Color = mode=="driving"?"orange":"blue", Locations = mapSettings.Markers.Select(marker => marker.Location).ToList() } };
            mapSettings.Center = mapSettings.Markers.First().Location;
            return mapSettings;
        }

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