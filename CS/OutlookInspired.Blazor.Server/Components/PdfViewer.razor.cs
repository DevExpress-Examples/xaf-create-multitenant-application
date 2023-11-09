namespace OutlookInspired.Blazor.Server.Components{
    public class PdfModel : ComponentModelBase<PdfViewer>{
        public byte[] Bytes{
            get => GetPropertyValue<byte[]>();
            set => SetPropertyValue( value);
        }
        public bool ToolBar{
            get => GetPropertyValue<bool>();
            set => SetPropertyValue( value);
        }
        public string Style{
            get => GetPropertyValue<string>();
            set => SetPropertyValue( value);
        }
    }
}