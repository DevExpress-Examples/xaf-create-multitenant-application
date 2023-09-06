using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Controllers.Maps{
    public interface IModelOptionsHomeOffice{
        IModelHomeOffice HomeOffice{ get; }    
    }
    
    public interface IModelHomeOffice:IMapsMarker,IModelNode{
        [DefaultValue("Glendale")]
        string City{ get; set; }
        [DefaultValue("91203")]
        string ZipCode{ get; set; }
        [DefaultValue("505 N. Brand Blvd")]
        string Line{ get; set; }
        [DefaultValue(StateEnum.CA)]
        StateEnum State{ get; set; }
        [DefaultValue(34.1532866)]
        new double Latitude{ get; set; }
        [DefaultValue(-118.2555815)]
        new double Longitude{ get; set; }
    }

    public interface IMapsRouteController{
        event EventHandler<RouteCalculatedArgs> RouteCalculated;
    }

    public class RouteCalculatedArgs:EventArgs{
        public RoutePoint[] RoutePoints{ get; }

        public RouteCalculatedArgs(RoutePoint[] routePoints,double distance,TimeSpan time){
            RoutePoints = routePoints;
            Distance = distance;
            Time = time;
        }

        public double Distance{ get;  }
        public TimeSpan Time{ get;  }
    }
}