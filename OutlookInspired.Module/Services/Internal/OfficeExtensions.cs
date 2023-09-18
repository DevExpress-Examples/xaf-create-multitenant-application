using DevExpress.Pdf;
using DevExpress.XtraRichEdit;

namespace OutlookInspired.Module.Services.Internal{
    internal static class OfficeExtensions{
        public static byte[] AddWaterMark(this byte[] bytes,string text){
            using var processor = new PdfDocumentProcessor();
            using var memoryStream = new MemoryStream(bytes);
            processor.LoadDocument(memoryStream);
            processor.AddWatermark(text);
            using var stream = new MemoryStream();
            processor.SaveDocument(stream);
            return stream.ToArray();
        }

        public static byte[] ToPdf(this byte[] bytes){
            using var richEditDocumentServer = new RichEditDocumentServer();
            richEditDocumentServer.LoadDocument(bytes);
            using var memoryStream = new MemoryStream();
            richEditDocumentServer.ExportToPdf(memoryStream);
            return memoryStream.ToArray();
        }

        public static RichEditDocumentServer CreateDocumentServer(this byte[] bytes, params object[] dataSource) 
            => new(){
                OpenXmlBytes = bytes,
                Options ={
                    MailMerge ={
                        DataSource = dataSource
                    }
                }
            };

    }
}