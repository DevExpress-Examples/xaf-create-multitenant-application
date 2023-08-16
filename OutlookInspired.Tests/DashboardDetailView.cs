using System.Collections;
using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraLayout;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Tests.ImportData.Extensions;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData{
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
                yield return new TestCaseData("EmployeeListView","EmployeeListView", DashboardDetailViewExtensions.AssertEmployeeDetailView);
                yield return new TestCaseData("EmployeeListView","EmployeeCardListView", DashboardDetailViewExtensions.AssertEmployeeDetailView);
                yield return new TestCaseData("CustomerListView","CustomerListView", DashboardDetailViewExtensions.AssertCustomerDetailView);
                yield return new TestCaseData("CustomerListView","CustomerCardListView", DashboardDetailViewExtensions.AssertCustomerDetailView);
                yield return new TestCaseData("ProductListView","ProductCardView", DashboardDetailViewExtensions.AssertProductDetailView);
                yield return new TestCaseData("ProductListView","ProductListView", DashboardDetailViewExtensions.AssertProductDetailView);
            }
        }
    }

    static class DashboardDetailViewExtensions{
        internal static IObservable<Unit> AssertCustomerDetailView(this XafApplication application, IObservable<DashboardViewItem> itemSource) 
            => itemSource.AssertDetailViewGridControlHasObjects().ToUnit();

        internal static IObservable<Unit> AssertProductDetailView(this XafApplication application, IObservable<DashboardViewItem> itemSource) 
            => itemSource.AssertDetailViewPdfViewerHasPages();
        
        internal static IObservable<Unit> AssertEmployeeDetailView(this XafApplication application,IObservable<DashboardViewItem> itemSource){
            var tabControl = application.AssertTabControl<TabbedGroup>();
            return itemSource.Merge(tabControl.IgnoreElements().To<DashboardViewItem>()).BufferUntilCompleted()
                .SelectMany(items => items.AssertListView(typeof(Evaluation))
                    .ConcatToUnit(tabControl.Do(group => group.SelectedTabPageIndex = 1))
                    .ConcatToUnit(items.AssertListView(typeof(EmployeeTask))))
                .ToUnit();
        }

        
    }
}