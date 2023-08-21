namespace OutlookInspired.Module.Attributes{
    public enum CloneViewType{
        DetailView,
        ListView,
        LookupListView
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CloneViewAttribute : Attribute{
        public CloneViewAttribute(CloneViewType viewType, string viewId){
            ViewType = viewType;
            ViewId = viewId;
        }

        public string ViewId{ get; }
        public CloneViewType ViewType{ get; }
        public string DetailView{ get; set; }
    }
}