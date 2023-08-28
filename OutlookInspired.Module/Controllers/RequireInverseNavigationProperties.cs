using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;

namespace OutlookInspired.Module.Controllers{
    [Obsolete]
    public class RequireAggregatedInverseNavigationPropertiesController:Controller{
        public override void CustomizeTypesInfo(ITypesInfo typesInfo){
            base.CustomizeTypesInfo(typesInfo);
            typesInfo.PersistentTypes.Where(info => typeof(OutlookInspiredBaseObject).IsAssignableFrom(info.Type))
                .SelectMany(info => info.AttributedMembers<AggregatedAttribute>())
                .Do(_ => {
                    // var associatedMemberInfo = t.memberInfo.AssociatedMemberInfo;
                    // var attribute = associatedMemberInfo.FindAttribute<RuleRequiredFieldAttribute>();
                    //
                    // if (attribute == null){
                    //     associatedMemberInfo.AddAttribute(new RuleRequiredFieldAttribute());    
                    // }
                })
                .Enumerate();
        }
    }
}