using System.Collections.ObjectModel;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Services{
    public interface IRouteMapsMarker:IMapsMarker{
        
    }

    public interface ISalesMapsMarker:IMapsMarker{
        IEnumerable<Order> Orders{ get; }
        
    }
}