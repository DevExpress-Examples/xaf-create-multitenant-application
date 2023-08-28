using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using DevExpress.ExpressApp;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;
using XAF.Testing.RX;
using XAF.Testing.XAF;

namespace OutlookInspired.Tests.ImportData{
    static class FilterListView{
        internal static IObservable<Frame> FilterEmployeeListViews<T>(this XafApplication application,IObservable<T> source) 
            => application.WhenListViewCreating(typeof(Employee))
                // .Do(t => t.e.CollectionSource.SetCriteria<Employee>(employee =>employee.Evaluations.Any()&& employee.AssignedTasks.Any(task => task.AttachedFiles.Any())))
                .Merge(application.WhenListViewCreating(typeof(EmployeeTask))
                    // .Do(t => t.e.CollectionSource.SetCriteria<EmployeeTask>(employeeTask =>employeeTask.AttachedFiles.Any()))
                )
                .ToFirst().To<Frame>()
                .TakeUntilCompleted(source);
        internal static IObservable<Frame> FilterEmployeeListViews(this XafApplication application) 
            => application.WhenListViewCreating(typeof(Employee))
                .Do(t => t.e.CollectionSource.SetCriteria<Employee>(employee =>employee.Evaluations.Any()&& employee.AssignedTasks.Any(task => task.AttachedFiles.Any())))
                .Merge(application.WhenListViewCreating(typeof(EmployeeTask))
                    .Do(t => t.e.CollectionSource.SetCriteria<EmployeeTask>(employeeTask =>employeeTask.AttachedFiles.Any()))
                )
                .ToFirst().To<Frame>();


        internal static IObservable<Frame> FilterListViews(this IObservable<Frame> source, XafApplication application)
            => source.Merge(application.FilterAllListViews(source));
        
        internal static IObservable<Frame> FilterAllListViews(this XafApplication application,IObservable<Frame> assert) 
            => application.FilterListViews((view, expression) => view.FilterUserControl( expression).ToObservable(),Expressions())
                .IgnoreElements().TakeUntilCompleted(assert).To<Frame>();

        private static LambdaExpression[] Expressions()
            => new LambdaExpression[]{
                Customers(), CustomerEmployees(), Orders(), Quotes(), CustomerStores(),
                Employees(),Tasks(),
                Products()
            };
        private static Expression<Func<Customer, bool>> Customers() 
            => customer => customer.Employees.Any() && customer.Orders.Any() && customer.Quotes.Any() && customer.CustomerStores.Any();
        private static Expression<Func<Product, bool>> Products() 
            => product => product.OrderItems.Any();
        
        private static Expression<Func<Employee, bool>> Employees() 
            => employee => employee.AssignedTasks.Any()&&employee.Evaluations.Any();
        
        private static Expression<Func<EmployeeTask, bool>> Tasks() 
            => employeeTask => employeeTask.AttachedFiles.Any()&&employeeTask.AssignedEmployees.Any();

        private static Expression<Func<CustomerStore, bool>> CustomerStores() 
            => store => store.CustomerEmployees.Any() && store.Orders.Any() && store.Quotes.Any();

        private static Expression<Func<Quote, bool>> Quotes() 
            => order => order.QuoteItems.Any();

        private static Expression<Func<Order, bool>> Orders() 
            => order => order.OrderItems.Any();

        private static Expression<Func<CustomerEmployee, bool>> CustomerEmployees() 
            => customerEmployee => customerEmployee.CustomerCommunications.Any();

        internal static IObservable<Frame> FilterCustomerListViews<T>(this XafApplication application, IObservable<T> source){
            var dashboardView = application.WhenFrame(ViewType.DashboardView).TakeAndReplay(1).RefCount();
            application.WhenFrame(typeof(Customer), ViewType.DetailView).Where(frame => frame.View.Id=="CustomerGridView_DetailView")
                .Select(frame => frame).Subscribe();
            return application.WhenFrame(typeof(Customer),ViewType.DetailView)
                .Buffer(dashboardView).SelectMany(frames => dashboardView.Select(frame => frame.View.ToDashboardView().MasterFrame())
                    .Select(masterFrame => frames.First(frame => frame==masterFrame)))
                .Select(frame => frame).Take(1)
                .ToDetailView().WhenControlsCreated()
                .Select(view => view.UserControl()).WhenNotDefault()
                .Do(control => control.SetCriteria<Customer>(customer => customer.Criteria()))
                .MergeToUnit(application.WhenListViewCreating(typeof(CustomerEmployee))
                    // .Do(t => t.e.CollectionSource.SetCriteria<CustomerEmployee>(customerEmployee => customerEmployee.Criteria()))
                )
                .MergeToUnit(application.WhenListViewCreating(typeof(Order))
                    // .Do(t => t.e.CollectionSource.SetCriteria<Order>(order => order.Criteria()))
                )
                .MergeToUnit(application.WhenListViewCreating(typeof(Quote))
                    // .Do(t => t.e.CollectionSource.SetCriteria<Quote>(order => order.Criteria()))
                )
                .MergeToUnit(application.WhenListViewCreating(typeof(CustomerStore))
                    // .Do(t => t.e.CollectionSource.SetCriteria<CustomerStore>(store => store.Criteria()))
                )
                .To<Frame>()
                .TakeUntilCompleted(source);
            
        }

        private static bool Criteria(this CustomerStore store) 
            => store.CustomerEmployees.Any(Criteria)&&store.Orders.Any(Criteria)&&store.Quotes.Any(Criteria);

        private static bool Criteria(this Quote order) 
            => order.QuoteItems.Any();

        private static bool Criteria(this Order order) 
            => order.OrderItems.Any();

        private static bool Criteria(this Customer customer) 
            => customer.Employees.Any(Criteria) && customer.Orders.Any(Criteria) && customer.Quotes.Any(Criteria) && customer.CustomerStores.Any(Criteria);

        private static bool Criteria(this CustomerEmployee employee) 
            => employee.CustomerCommunications.Any();
    }
}