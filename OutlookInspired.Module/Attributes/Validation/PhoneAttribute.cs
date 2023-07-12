using DevExpress.Persistent.Validation;

namespace OutlookInspired.Module.Attributes.Validation{
    public class PhoneAttribute:RuleRegularExpressionAttribute{
        public PhoneAttribute():base("^(http(s)?://)?([\\w-]+\\.)+[\\w-]+(/[\\w- ;,./?%&=]*)?$"){
        }
    }
}