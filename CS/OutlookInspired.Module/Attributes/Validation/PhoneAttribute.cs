using DevExpress.Persistent.Validation;

namespace OutlookInspired.Module.Attributes.Validation{
    public class PhoneAttribute:RuleRegularExpressionAttribute{
        public PhoneAttribute():base(@"^\(\d{3}\)\s\d{3}-\d{4}$"){
        }
    }
}