using DevExpress.ExpressApp.Data;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace OutlookInspired.Module.BusinessObjects{
    [DomainComponent]
    public class QuoteMapItem:IMapsMarker{
        [Key]
        public int Key{ get; set; }
        public Stage Stage { get; init; }
        public DateTime Date { get; set; }
        public string City{ get; set; }
        string IBaseMapsMarker.Title => City;
        public double Latitude{ get; set; }
        public double Longitude { get; set; }
        public string Name => Enum.GetName(typeof (Stage), Stage);
        public int Index => (int) Stage;
        public Decimal Value { get; set; }
    }
    
    public enum Stage{
        [ImageName(nameof(High))]
        High,
        [ImageName(nameof(Medium))]
        Medium,
        [ImageName(nameof(Low))]
        Low,
        [ImageName("Unlike")]
        Unlikely,
        [ImageName(nameof(Summary))]
        Summary
    }

}