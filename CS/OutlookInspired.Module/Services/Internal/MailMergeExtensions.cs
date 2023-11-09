using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Office;
using DevExpress.Office.Services;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using OutlookInspired.Module.BusinessObjects;

namespace OutlookInspired.Module.Services.Internal{
    static class MailMergeExtensions{
        public const string MailMergeOrder="Order";
        public const string MailMergeOrderItem="OrderItem";
        public const string FollowUp="FollowUp";
        public const string ProbationNotice="Probation Notice";
        public const string ServiceExcellence="Service Excellence";
        public const string ThankYouNote="Thank You Note";
        public const string WelcomeToDevAV="Welcome to DevAV";
        public const string MonthAward="Month Award";
        public static byte[] MailMergeInvoice(this Order order) 
            => order.ObjectSpace.MailMergeData("Order").Template.CreateDocumentServer(order).MailMergeInvoice(order);

        static byte[] MailMergeInvoice(this IRichEditDocumentServer richEditDocumentServer,Order order){
            richEditDocumentServer.CalculateDocumentVariable += (_, e) => e.CalculateDocumentVariable(order, richEditDocumentServer);
            return richEditDocumentServer.MailMerge(order);
        }
        public static void ApplyMailMergeProtection(this SingleChoiceAction action, Func<ChoiceActionItem,bool> match) 
            => action.Items.SelectManyRecursive(item => item.Items)
                .WhereNotDefault(item => item.Data).Where(match)
                .Do(item => item.Enabled[nameof(ApplyMailMergeProtection)] = action.View().ObjectSpace.GetObjectsQuery<RichTextMailMergeData>()
                    .Any(data => data.Name == (string)item.Data))
                .Enumerate();

        public static void CreateMailMergeTemplates(this IObjectSpace objectSpace) 
            => new[]{
                    (type: typeof(Order), name: FollowUp), (type: typeof(Order), name: MailMergeOrder), (type: typeof(OrderItem), name: MailMergeOrderItem),
                    (type: typeof(Employee), name: ProbationNotice),(type: typeof(Employee), name: ServiceExcellence),(type: typeof(Employee), name: ThankYouNote)
                    ,(type: typeof(Employee), name: WelcomeToDevAV),(type: typeof(Employee), name: MonthAward),
                }
                .Do(t => objectSpace.NewMailMergeData(t.name,t.type,typeof(MailMergeExtensions).Assembly
                    .GetManifestResourceStream(s => s.Contains("MailMerge")  && s.EndsWith($"{t.name}.docx")).Bytes()))
                .Enumerate();

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
            using var mergedServer = mailMergeData.Template.CreateDocumentServer(dataSource);
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
        
    }
}