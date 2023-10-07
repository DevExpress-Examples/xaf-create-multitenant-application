using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components.Models;
using Microsoft.AspNetCore.Components;
using OutlookInspired.Blazor.Server.Services;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.Maps;
using OutlookInspired.Module.Services.Internal;
using FeatureCollection = OutlookInspired.Blazor.Server.Services.FeatureCollection;

namespace OutlookInspired.Blazor.Server.Components.DevExtreme{
    
    public class DxMapModel:ComponentModelBase,IComponentContentHolder{
        public event EventHandler<MapItemSelectedArgs> MapItemSelected;
        public MapSettings MapSettings{
            get => GetPropertyValue<MapSettings>();
            set => SetPropertyValue(value);
        }

        public bool ChangeRouteMode{ get; set; }
        public bool ChangePeriod{ get; set; }
        public bool PrintMap{
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }

        public void SelectMapItem(MapItem item) 
            => MapItemSelected?.Invoke(this, new MapItemSelectedArgs(item));
        public RenderFragment ComponentContent => this.Create(model => model.Create<DxMap>());
    }

    public class MapItemSelectedArgs:EventArgs{
        public MapItem Item{ get; }

        public MapItemSelectedArgs(MapItem item) => Item = item;
    }

    
public class Location{
        public double Lat{ get; init; }
        public double Lng{ get; init; }
        
    }

    public class MapMarker{
        public Location Location{ get; init; }
    }

    public class MapRoute{
        public int Weight{ get; set; } = 6;
        public string Color{ get; set; } = "orange";
        public double Opacity{ get; set; } = 0.5;
        public string Mode{ get; init; }
        public List<Location> Locations{ get; set; } = new();
    }

    public class MapSettings{
        private string[] _palette;
        private FeatureCollection _features;
        private MapItem[] _mapItems;

        public static MapSettings New(ISalesMapsMarker marker,Period period) {
            var mapItems = marker.Sales(period).ToArray();
            var palette = mapItems.Select(item => item.PropertyValue(marker.GetType())).Distinct().Count().DistinctColors().ToArray();
            return new(){
                _mapItems = mapItems.Colorize(palette,marker.GetType()),
                _palette = palette,
                _features = (FeatureCollection)mapItems.Features()
            };
        }

        public Location Center{ get; set; }
        public string Provider{ get; set; } = "bing";
        public FeatureCollection Features => _features; 
        public string ApiKey{ get; set; } = "AgPa0XVf4_HaN5BOPbTUw5KNvYEGOx-EftnjNRnCILfNgobxJC_deESiKqcfEgLd";
        public int Zoom{ get; set; } = 16;
        public bool Controls{ get; set; } = true;

        public double[] Bounds 
            =>MapItems!=null? (MapItems.Min(item => item.Longitude) -
                (MapItems.Max(item => item.Longitude) - MapItems.Min(item => item.Longitude)) * 0.1).YieldItem()
            .Concat(MapItems.Max(item => item.Latitude) + (MapItems.Max(item => item.Latitude) - MapItems.Min(item => item.Latitude)) * 0.1)
            .Concat(MapItems.Max(item => item.Longitude) + (MapItems.Max(item => item.Longitude) - MapItems.Min(item => item.Longitude)) * 0.1)
            .Concat(MapItems.Min(item => item.Latitude) - (MapItems.Max(item => item.Latitude) - MapItems.Min(item => item.Latitude)) * 0.1 ).ToArray():null;

        public List<MapMarker> Markers{ get; set; } = new();
        public List<MapRoute> Routes{ get; set; } = new();
        public MapItem[] MapItems => _mapItems;
        public string[] Palette => _palette;
    }

}