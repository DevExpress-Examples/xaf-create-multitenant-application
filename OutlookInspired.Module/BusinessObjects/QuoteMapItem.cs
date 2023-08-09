using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace OutlookInspired.Module.BusinessObjects{
    [DomainComponent]
    public class QuoteMapItem{
        public Stage Stage { get; init; }
        public DateTime Date { get; set; }
        public string City{ get; set; }
        public double Latitude{ get; set; }
        public double Longitude { get; set; }
        public string Name => Enum.GetName(typeof (Stage), Stage);
        public int Index => (int) Stage;
        public Decimal Value { get; set; }
    }
    
    public enum Stage{
        High, Medium, Low, Unlikely, Summary,
    }

}