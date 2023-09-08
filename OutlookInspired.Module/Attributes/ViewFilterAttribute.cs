namespace OutlookInspired.Module.Attributes{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewFilterAttribute:Attribute{
        public ViewFilterAttribute(string currentFilter="All") => CurrentFilter = currentFilter;

        public string CurrentFilter{ get; }
    }
}