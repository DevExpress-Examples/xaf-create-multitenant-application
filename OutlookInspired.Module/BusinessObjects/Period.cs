using DevExpress.Persistent.Base;

namespace OutlookInspired.Module.BusinessObjects{
    public enum Period{
        [ImageName("SalesAnalysis")]
        ThisYear,
        [ImageName("CustomerQuickSales")]
        ThisMonth,
        [ImageName("Demo_SalesOverview")]
        Lifetime,
        FixedDate,
    }
}