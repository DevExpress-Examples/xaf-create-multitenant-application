using DevExpress.Persistent.Base;

namespace OutlookInspired.Module.BusinessObjects{
    public enum Period{
        [ImageName("Demo_SalesOverview")]
        Lifetime,
        [ImageName("SalesAnalysis")]
        ThisYear,
        [ImageName("CustomerQuickSales")]
        ThisMonth, FixedDate,
    }
}