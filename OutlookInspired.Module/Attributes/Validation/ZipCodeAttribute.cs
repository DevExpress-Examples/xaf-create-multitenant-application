using DevExpress.Persistent.Validation;

namespace OutlookInspired.Module.Attributes.Validation{
    public class ZipCodeAttribute:RuleRegularExpressionAttribute{
        public ZipCodeAttribute() : base(@"^[0-9][0-9][0-9][0-9][0-9]$") => CustomMessageTemplate = "The {0} field is not a valid ZIP code.";
    }
}