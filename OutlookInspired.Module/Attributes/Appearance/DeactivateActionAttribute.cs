using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

namespace OutlookInspired.Module.Attributes.Appearance{
    public class DeactivateActionAttribute:AppearanceAttribute{
        public DeactivateActionAttribute(params string[] actions) :
            base($"Deactivate {actions.StringJoin(" ")}",DevExpress.ExpressApp.ConditionalAppearance.AppearanceItemType.Action,"1=1"){
            Visibility=ViewItemVisibility.Hide;
            TargetItems = actions.StringJoin(";");
        }
    }
}