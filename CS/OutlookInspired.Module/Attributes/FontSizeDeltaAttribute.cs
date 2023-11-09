namespace OutlookInspired.Module.Attributes{
    [AttributeUsage(AttributeTargets.Property)]
    public class FontSizeDeltaAttribute:Attribute{
        public int Delta{ get; }

        public FontSizeDeltaAttribute(int delta) => Delta = delta;
    }
}