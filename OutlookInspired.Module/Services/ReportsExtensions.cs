using System.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Office;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Office.Services;
using DevExpress.Pdf;
using DevExpress.Persistent.Base;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Resources.Reports;
using MergeMode = DevExpress.XtraRichEdit.API.Native.MergeMode;

namespace OutlookInspired.Module.Services;

static class ReportsExtensions {
    public static PredefinedReportsUpdater AddOrderReports(this PredefinedReportsUpdater predefinedReportsUpdater){
        predefinedReportsUpdater.AddPredefinedReport<FedExGroundLabel>(nameof(FedExGroundLabel), typeof(Order));
        predefinedReportsUpdater.AddPredefinedReport<SalesRevenueReport>("Revenue Report", typeof(Order));
        predefinedReportsUpdater.AddPredefinedReport<SalesRevenueAnalysisReport>("Revenue Analysis", typeof(Order));
        return predefinedReportsUpdater;
    }

    public static PredefinedReportsUpdater AddCustomerReports(this PredefinedReportsUpdater predefinedReportsUpdater){
        predefinedReportsUpdater.AddPredefinedReport<CustomerContactsDirectory>("Contacts", typeof(Customer));
        predefinedReportsUpdater.AddPredefinedReport<CustomerLocationsDirectory>("Locations", typeof(Customer));
        predefinedReportsUpdater.AddPredefinedReport<CustomerSalesDetailReport>("Sales Detail Report", typeof(Customer));
        predefinedReportsUpdater.AddPredefinedReport<CustomerSalesDetail>("Sales Detail", typeof(Customer));
        predefinedReportsUpdater.AddPredefinedReport<CustomerSalesSummary>("Sales Summary", typeof(Customer));
        predefinedReportsUpdater.AddPredefinedReport<CustomerSalesSummaryReport>("Sales Summary Report", typeof(Customer));
        predefinedReportsUpdater.AddPredefinedReport<CustomerProfile>("Profile", typeof(Customer));
        return predefinedReportsUpdater;
    }
    public static PredefinedReportsUpdater AddProductReports(this PredefinedReportsUpdater predefinedReportsUpdater){
        predefinedReportsUpdater.AddPredefinedReport<ProductOrders>("Orders", typeof(Product));
        predefinedReportsUpdater.AddPredefinedReport<ProductProfile>("Profile", typeof(Product));
        predefinedReportsUpdater.AddPredefinedReport<ProductSalesSummary>("Sales", typeof(Product));
        predefinedReportsUpdater.AddPredefinedReport<ProductTopSalesperson>("Top Sales Person", typeof(Product));
        return predefinedReportsUpdater;
    }


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
        var stream = new MemoryStream();
        documentServer.GetService<IUriStreamService>().RegisterProvider(new ImageStreamProviderBase(
            documentServer.Options.MailMerge, datasource,
            XafTypesInfo.Instance.FindTypeInfo(typeof(T))));
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

    public static byte[] ToPdf(this XtraReport report,string waterMarkText=null){
        using var memoryStream = new MemoryStream();
        report.ExportToPdf(memoryStream);
        var bytes = memoryStream.ToArray();
        return waterMarkText != null ? bytes.AddWaterMark(waterMarkText) : bytes;
    }

    public static string WatermarkText(this Order order) 
        => order.ShipmentStatus switch{
            ShipmentStatus.Received => "Shipment Received",
            ShipmentStatus.Transit => "Shipment in Transit",
            _ => "Awaiting shipment"
        };

    public static void AddWatermark(this PdfDocumentProcessor processor, string watermark){
        var pages = processor.Document.Pages;
        using Font font = new Font("Segoe UI", 48, FontStyle.Regular);
        foreach (var t in pages){
            using var graphics = processor.CreateGraphics();
            var pageLayout = new RectangleF(
                -(float)t.CropBox.Width * 0.35f,
                (float)t.CropBox.Height * 0.1f,
                (float)t.CropBox.Width * 1.25f,
                (float)t.CropBox.Height);
                        
            var angle = Math.Asin(pageLayout.Width / (double)pageLayout.Height) * 180.0 / Math.PI;
            graphics.TranslateTransform(-pageLayout.X, -pageLayout.Y);
            graphics.RotateTransform((float)angle);

            using(SolidBrush textBrush = new SolidBrush(Color.FromArgb(100, Color.Red)))
                graphics.DrawString(watermark, font, textBrush, new PointF(50, 50));
            graphics.AddToPageForeground(t);
        }
    }

    static byte[] AddWaterMark(this byte[] bytes,string text){
        using var processor = new PdfDocumentProcessor();
        using var memoryStream = new MemoryStream(bytes);
        processor.LoadDocument(memoryStream);
        processor.AddWatermark(text);
        using var stream = new MemoryStream();
        processor.SaveDocument(stream);
        return stream.ToArray();
    }
}