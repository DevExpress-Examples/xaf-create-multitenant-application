namespace OutlookInspired.Module.Attributes.Appearance{
    public class ForbidDeleteAttribute:DeactivateActionAttribute{
        public ForbidDeleteAttribute(params string[] contexts) : base("Delete") 
            => Context = string.Join(";", contexts);
    }
}