namespace OutlookInspired.Module.BusinessObjects{
    public class ProductCatalog :MyBaseObject{
        public virtual Product Product { get; set; }
        public  virtual long? ProductId { get; set; }
        public  virtual byte[] PDF { get; set; }
        Stream _pdfStream;
        public Stream PdfStream => _pdfStream ??= new MemoryStream(PDF);
    }
}