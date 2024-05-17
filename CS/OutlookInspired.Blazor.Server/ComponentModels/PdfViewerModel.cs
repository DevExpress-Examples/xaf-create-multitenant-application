using OutlookInspired.Blazor.Server.Components;

namespace OutlookInspired.Blazor.Server.ComponentModels {
    public class PdfViewerModel : ComponentModelBase {
        public byte[] Bytes {
            get => GetPropertyValue<byte[]>();
            set => SetPropertyValue(value);
        }
        public bool ToolBar {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }
        public string Style {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }
        public override Type ComponentType => typeof(PdfViewer);
    }
}
