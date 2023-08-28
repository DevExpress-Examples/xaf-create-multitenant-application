using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Validation;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers{
    public class RequireAggregatedInverseNavigationPropertiesController:Controller{
        public override void CustomizeTypesInfo(ITypesInfo typesInfo){
            base.CustomizeTypesInfo(typesInfo);
            typesInfo.PersistentTypes.Where(info => typeof(OutlookInspiredBaseObject).IsAssignableFrom(info.Type))
                .SelectMany(info => info.AttributedMembers<AggregatedAttribute>())
                .Do(t => {
                    var associatedMemberInfo = t.memberInfo.AssociatedMemberInfo;
                    if (associatedMemberInfo.FindAttribute<RuleRequiredFieldAttribute>() == null){
                        associatedMemberInfo.AddAttribute(new RuleRequiredFieldAttribute());    
                    }
                })
                .Enumerate();
        }
    }
}