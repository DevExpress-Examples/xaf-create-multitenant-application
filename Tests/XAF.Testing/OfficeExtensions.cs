using System.IO;
using DevExpress.XtraRichEdit;

namespace XAF.Testing{
    public static class OfficeExtensions{
        public static IRichEditDocumentServer Load(this IRichEditDocumentServer server,byte[] bytes,DocumentFormat? documentFormat=null){
            using var memoryStream = new MemoryStream(bytes);
            server.LoadDocument(memoryStream, documentFormat??DocumentFormat.Undefined);
            return server;
        }

    }
}