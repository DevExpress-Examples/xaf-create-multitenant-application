using System.Text.RegularExpressions;
using DevExpress.ExpressApp.DC;

namespace OutlookInspired.Module.BusinessObjects{
    [DomainComponent]
    public class RoutePoint {
        // static readonly Regex RemoveTagRegex = new(@"<[^>]*>", RegexOptions.Compiled);
        
        public RoutePoint() {
            // _item = item;
            // ManeuverInstruction = RemoveTagRegex.Replace(item.ManeuverInstruction, string.Empty);
            // double itemDistance = item.Distance;
            // Distance = (itemDistance > 0.9) ? $"{Math.Ceiling(itemDistance):0} mi"
            //     : $"{Math.Ceiling(itemDistance * 52.8) * 100:0} ft";
        }
        public BingManeuverType Maneuver{ get; set; }

        public string ManeuverInstruction{ get; set; }
        public string Distance{ get; set; }
    }

    public enum BingManeuverType{
        None,
        Unknown,
        DepartStart,
        DepartIntermediateStop,
        DepartIntermediateStopReturning,
        ArriveFinish,
        ArriveIntermediateStop,
        TurnLeft,
        TurnRight,
        TurnBack,
        UTurn,
        TurnToStayLeft,
        TurnToStayRight,
        BearLeft,
        BearRight,
        KeepToStayLeft,
        KeepToStayRight,
        KeepToStayStraight,
        KeepLeft,
        KeepRight,
        KeepStraight,
        Take,
        TakeRampLeft,
        TakeRampRight,
        TakeRampStraight,
        KeepOnrampLeft,
        KeepOnrampRight,
        KeepOnrampStraight,
        Merge,
        Continue,
        RoadNameChange,
        EnterRoundabout,
        ExitRoundabout,
        TurnRightThenTurnRight,
        TurnRightThenTurnLeft,
        TurnRightThenBearRight,
        TurnRightThenBearLeft,
        TurnLeftThenTurnLeft,
        TurnLeftThenTurnRight,
        TurnLeftThenBearLeft,
        TurnLeftThenBearRight,
        BearRightThenTurnRight,
        BearRightThenTurnLeft,
        BearRightThenBearRight,
        BearRightThenBearLeft,
        BearLeftThenTurnLeft,
        BearLeftThenTurnRight,
        BearLeftThenBearRight,
        BearLeftThenBearLeft,
        RampThenHighwayRight,
        RampThenHighwayLeft,
        RampToHighwayStraight,
        EnterThenExitRoundabout,
        BearThenMerge,
        TurnThenMerge,
        BearThenKeep,
        Transfer,
        Wait,
        TakeTransit,
        Walk,
        TurnLeftSharp,
        TurnRightSharp
    }

}