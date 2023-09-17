using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Features.Maps{
    public interface ITravelModeMapsMarker:IRouteMapsMarker{
        
    }
    public interface IRouteMapsMarker:IMapsMarker{
        
    }

    public interface ISalesMapsMarker:IMapsMarker,IObjectSpaceLink{
        IEnumerable<Order> Orders{ get; }
        
    }
}