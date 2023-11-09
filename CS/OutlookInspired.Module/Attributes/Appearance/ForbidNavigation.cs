namespace OutlookInspired.Module.Attributes.Appearance{
    public class ForbidNavigation:DeactivateActionAttribute{
        public ForbidNavigation() : base("PreviousObject","NextObject"){
            
        }
    }
}