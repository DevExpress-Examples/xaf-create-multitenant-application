// using DevExpress.XtraGrid.Views.Base;
// using OutlookInspired.Module.Controllers;
//
// namespace OutlookInspired.Win.UserControls
// {
//     internal static class UserControlExtensions
//     {
//         public static void SubscribeEvents(this IMasterDetailService masterDetailService,ColumnView columnView) {
//             columnView.FocusedRowObjectChanged += (_, e) => masterDetailService.SelectedObjectChanged?.Invoke(this, new ObjectArgs(e.Row));
//             columnView.DoubleClick += (_, _) => ProcessObject?.Invoke(this, new ObjectArgs(columnView.FocusedRowObject));
//         }
//
//     }
// }
