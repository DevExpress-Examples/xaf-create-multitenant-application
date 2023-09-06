using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Office;
using DevExpress.Office.Services;
using DevExpress.Persistent.Base;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Services{
    static class MailMergeExtensions{
        public static byte[] MailMergeInvoice(this Order order) 
            => order.ObjectSpace.MailMergeData("Order").CreateDocumentServer(order).MailMergeInvoice(order);

        static byte[] MailMergeInvoice(this IRichEditDocumentServer richEditDocumentServer,Order order){
            richEditDocumentServer.CalculateDocumentVariable += (_, e) => e.CalculateDocumentVariable(order, richEditDocumentServer);
            return richEditDocumentServer.MailMerge(order);
        }

        private static void CalculateDocumentVariable(this CalculateDocumentVariableEventArgs e,Order order, IRichEditDocumentServer richEditDocumentServer){
            switch (e.VariableName){
                case nameof(Order.OrderItems):
                    richEditDocumentServer.MailMerge(order.ObjectSpace.MailMergeData("OrderItem"), MergeMode.JoinTables,
                        order.OrderItems.ToArray());
                    e.PreserveInsertedContentFormatting = true;
                    e.KeepLastParagraph = false;
                    e.Value = richEditDocumentServer;
                    e.Handled = true;
                    break;
                case "Total":
                    e.Value = order.OrderItems.TotalSum(x => x.Total);
                    e.Handled = true;
                    break;
                case "TotalDue":
                    e.Value = order.OrderItems.TotalSum(x => x.Total) + order.ShippingAmount;
                    e.Handled = true;
                    break;
            }
        }
    
        public static void MailMerge<T>(this IRichEditDocumentServer documentServer,IRichTextMailMergeData mailMergeData, MergeMode mergeMode,params T[] dataSource){
            using var mergedServer = mailMergeData.CreateDocumentServer(dataSource);
            using var memoryStream = new MemoryStream(mailMergeData.Template);
            mergedServer.LoadDocumentTemplate(memoryStream, DocumentFormat.OpenXml);
            mergedServer.Options.MailMerge.DataSource = dataSource;
            var options = mergedServer.Document.CreateMailMergeOptions();
            options.MergeMode = mergeMode;
            mergedServer.MailMerge(options, documentServer.Document);
        }

        public static byte[] MailMerge<T>(this IRichEditDocumentServer documentServer,params T[] datasource){
            using var stream = new MemoryStream();
            documentServer.GetService<IUriStreamService>().RegisterProvider(new ImageStreamProviderBase(
                documentServer.Options.MailMerge, datasource, XafTypesInfo.Instance.FindTypeInfo(typeof(T))));
            documentServer.MailMerge(documentServer.CreateMailMergeOptions(), stream, DocumentFormat.OpenXml);
            return stream.ToArray();
        }

        public static RichEditDocumentServer CreateDocumentServer(this IRichTextMailMergeData richTextMailMergeData, params object[] dataSource) 
            => new(){
                OpenXmlBytes = richTextMailMergeData.Template,
                Options ={
                    MailMerge ={
                        DataSource = dataSource
                    }
                }
            };

        public static byte[] ToPdf(this byte[] bytes){
            using var richEditDocumentServer = new RichEditDocumentServer();
            richEditDocumentServer.LoadDocument(bytes);
            using var memoryStream = new MemoryStream();
            richEditDocumentServer.ExportToPdf(memoryStream);
            return memoryStream.ToArray();
        }
    }
}