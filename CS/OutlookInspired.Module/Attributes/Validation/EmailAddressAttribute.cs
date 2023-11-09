using DevExpress.Persistent.Validation;

namespace OutlookInspired.Module.Attributes.Validation{
    public class EmailAddressAttribute:RuleRegularExpressionAttribute{
        public EmailAddressAttribute():base(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"){
        }
    }
}