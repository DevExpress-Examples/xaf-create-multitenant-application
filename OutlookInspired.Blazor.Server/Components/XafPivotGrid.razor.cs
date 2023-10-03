using DevExpress.Blazor;

namespace OutlookInspired.Blazor.Server.Components {
    public class XafPivotGridModel<T> {
        public IEnumerable<T> Data { get; set; }
        public IEnumerable<XafPivotGridFieldModel> Fields { get; init; }
        public Action Update { get; set; }
    }
    public class XafPivotGridFieldModel {
        public string Name { get; init; }
        public PivotGridFieldArea Area { get; init; }
        public string Caption{ get; init; }
    }
}
