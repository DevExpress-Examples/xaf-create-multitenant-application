

using System.ComponentModel;

namespace OutlookInspired.Module.BusinessObjects{
    public class ProductCatalog :MigrationBaseObject{
        public virtual Product Product { get; set; }
        [Browsable(false)]
        public virtual Guid? ProductId { get; set; }
        public  virtual byte[] PDF { get; set; }
        Stream _pdfStream;
        public Stream PdfStream => _pdfStream ??= new MemoryStream(PDF);
    }
}