using System.Collections;
using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.XtraLayout;
using NUnit.Framework;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData{
    [Apartment(ApartmentState.STA)]
    public class DashboardDetailView:TestBase{
        [TestCaseSource(nameof(TestCases))]
        public async Task Test(string viewId,string viewVariant,Func<XafApplication,IObservable<DashboardViewItem>,IObservable<Unit>> assert){
            
            using var application = await SetupWinApplication(viewVariant);
            application.Model.Options.UseServerMode = false;
            
            // application.StartWinTest(assert(application, application.AssertDashboardDetailView(viewId, viewVariant)));
        }

        private static IEnumerable TestCases{
            get{
                yield return new TestCaseData("EmployeeListView","EmployeeListView", DashboardDetailViewExtensions.AssertEmployeeDetailView);
                // yield return new TestCaseData("EmployeeListView","EmployeeCardListView", DashboardDetailViewExtensions.AssertEmployeeDetailView);
                // yield return new TestCaseData("CustomerListView","CustomerListView", DashboardDetailViewExtensions.AssertCustomerDetailView);
                // yield return new TestCaseData("CustomerListView","CustomerCardListView", DashboardDetailViewExtensions.AssertCustomerDetailView);
                // yield return new TestCaseData("ProductListView","ProductCardView", DashboardDetailViewExtensions.AssertProductDetailView);
                // yield return new TestCaseData("ProductListView","ProductListView", DashboardDetailViewExtensions.AssertProductDetailView);
            }
        }
    }

    static class DashboardDetailViewExtensions{
        

        internal static IObservable<Unit> AssertProductDetailView(this XafApplication application, IObservable<DashboardViewItem> itemSource) 
            => itemSource.AssertDetailViewPdfViewerHasPages();
        
        internal static IObservable<Unit> AssertEmployeeDetailView(this XafApplication application,IObservable<DashboardViewItem> itemSource){
            var tabControl = application.AssertTabControl<TabbedGroup>();
            return itemSource.Merge(tabControl.IgnoreElements().To<DashboardViewItem>()).BufferUntilCompleted()
                .SelectMany(items => items.AssertListView(typeof(Evaluation))
                    .ConcatToUnit(tabControl.Do(group => group.SelectedTabPageIndex = 1))
                    .ConcatToUnit(application.AssertTaskViews(items)))
                .ToUnit();
        }

        private static IObservable<Unit> AssertTaskViews(this XafApplication application, DashboardViewItem[] items){
            throw new NotImplementedException();
            // var assertTabControl = application.AssertTabControl<TabbedGroup>();
            // return assertTabControl.MergeToUnit(items.AssertListView(typeof(EmployeeTask),
            //     frame => frame.View.ToDetailView().NestedListViews(typeof(Employee)).Take(1)
            //         .MergeToUnit(assertTabControl.Do(group => group.SelectedTabPageIndex = 2).ToUnit()
            //             // .IgnoreElements()
            //         )
            //         
            //         // .ConcatToUnit(frame.Observe().AssertListView().DoNotComplete())
            //     ));
            
        }
    }
}