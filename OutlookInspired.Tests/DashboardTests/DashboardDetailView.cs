using System.Collections;
using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Testing.DevExpress.ExpressApp;
using DevExpress.ExpressApp.Testing.RXExtensions;
using DevExpress.XtraLayout;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.ImportData.Extensions;

namespace OutlookInspired.Tests.ImportData.DashboardTests{
    [Apartment(ApartmentState.STA)]
    public class DashboardDetailView:TestBase{
        [TestCaseSource(nameof(TestCases))]
        public async Task Test(string viewId,string viewVariant,Func<XafApplication,IObservable<DashboardViewItem>,IObservable<Unit>> assert){
            
            using var application = await SetupWinApplication(viewVariant);
            application.Model.Options.UseServerMode = false;
            
            application.StartWinTest(assert(application, application.AssertDashboardDetailView(viewId, viewVariant)));
        }

        private static IEnumerable TestCases{
            get{
                yield return new TestCaseData("EmployeeListView","EmployeeListView", AssertEmployeeDetailView);
                yield return new TestCaseData("EmployeeListView","EmployeeCardListView", AssertEmployeeDetailView);
                yield return new TestCaseData("CustomerListView","CustomerListView", AssertCustomerDetailView);
                yield return new TestCaseData("CustomerListView","CustomerCardListView", AssertCustomerDetailView);
                yield return new TestCaseData("ProductListView","ProductCardView", AssertProductDetailView);
                yield return new TestCaseData("ProductListView","ProductListView", AssertProductDetailView);
            }
        }

        private static IObservable<Unit> AssertCustomerDetailView(XafApplication application, IObservable<DashboardViewItem> itemSource) 
            => itemSource.AssertDetailViewGridControlHasObjects().ToUnit();

        private static IObservable<Unit> AssertProductDetailView(XafApplication application, IObservable<DashboardViewItem> itemSource) 
            => itemSource.AssertDetailViewPdfViewerHasPages();

        private static IObservable<Unit> AssertEmployeeDetailView(XafApplication application,IObservable<DashboardViewItem> itemSource){
            var tabControl = application.AssertTabControl<TabbedGroup>();
            var evaluationsExist = application.AssertListViewHasObjects(typeof(Evaluation));
            var tasksExist = application.AssertListViewHasObjects(typeof(EmployeeTask));
            return itemSource.MergeToUnit(tabControl)
                .Merge(evaluationsExist.Concat(tabControl.Do(group => group.SelectedTabPageIndex = 1).ToUnit()))
                .Merge(tasksExist);        
        }
    }
}