using DevExpress.XtraCharts;

namespace OutlookInspired.Win.Controllers.Maps{
    public class Colorizer : DevExpress.XtraMap.KeyColorColorizer, IColorizer {
        
        event System.ComponentModel.PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged {
            add { }
            remove { }
        }

        public Color GetPointColor(object argument, object[] values, object colorKey, Palette palette) 
            => colorKey != null ? GetColor(colorKey) : Color.Empty;

        public Color GetPointColor(object argument, object[] values, object[] colorKeys, Palette palette) 
            => colorKeys is{ Length: > 0 } ? GetColor(colorKeys[0]) : Color.Empty;

        public Color GetAggregatedPointColor(object argument, object[] values, SeriesPoint[] points, Palette palette) 
            => Color.Empty;
    }
}