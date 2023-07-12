using System.ComponentModel;
using OutlookInspired.Module.Attributes.Validation;

namespace OutlookInspired.Module.BusinessObjects{
    public class Address :BaseObject {
        [DisplayName("Address")]
        public virtual string Line { get; set; }
        public virtual string City { get; set; }
        public virtual StateEnum State { get; set; }
        [ZipCode]
        public virtual string ZipCode { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
        public string CityLine => $"{City}, {State} {ZipCode}";
        public override string ToString() => $"{Line}, {CityLine}";
         
    }
    
    public enum StateEnum {
        CA, AR, AL, AK, AZ, CO, CT, DE, DC, FL, GA, HI, ID, IL, IN, IA, KS, KY, LA, ME, MD, MA, MI, MN, MS, MO, MT, NE, NV, NH, NJ, NM, NY, NC, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VT, VA, WA, WV, WI, WY, ND
    }
}