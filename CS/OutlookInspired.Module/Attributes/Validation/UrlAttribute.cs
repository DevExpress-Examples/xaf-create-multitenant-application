using DevExpress.Persistent.Validation;

namespace OutlookInspired.Module.Attributes.Validation{
    public class UrlAttribute:RuleRegularExpressionAttribute{
        public UrlAttribute() : base("^(http(s)?://)?([\\w-]+\\.)+[\\w-]+(/[\\w- ;,./?%&=]*)?$"){
        }
    }
}