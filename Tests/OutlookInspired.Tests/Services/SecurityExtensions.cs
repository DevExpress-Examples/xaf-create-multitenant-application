using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Services;
using XAF.Testing;
using XAF.Testing.XAF;
using NotImplementedException = System.NotImplementedException;

namespace OutlookInspired.Tests.Services{
    public static class SecurityExtensions{

        public static int AssertDashboardViewShowInDocumentActionItems(this SingleChoiceAction action)
            => action.UseCurrentEmployee(employee => employee?.Department switch{
                null=>5, EmployeeDepartment.Engineering or EmployeeDepartment.Sales => 3
                , EmployeeDepartment.Management => 1, EmployeeDepartment.HumanResources => 2,
                _ => throw new NotImplementedException($"department:{employee.Department}")
            });
        public static int AssertReportActionItems(this SingleChoiceAction action,ChoiceActionItem item=null)
            => action.UseCurrentEmployee(employee => action.View().ObjectTypeInfo.Type switch {
                { } t when t == typeof(Customer) => action.AssertCustomerReportActionItems(employee),
                { } t when t == typeof(Product) => action.AssertProductReportActionItems(employee),
                { } t when t == typeof(Order) => action.AssertOrderReportActionItems(employee,item),
                _ => throw new NotImplementedException(action.View().ObjectTypeInfo.Type.Name)
            });

        private static int AssertOrderReportActionItems(this SingleChoiceAction action, Employee employee, ChoiceActionItem item) 
            => employee?.Department switch{
                EmployeeDepartment.Shipping when item is{ Data: null }=>0,
                null or EmployeeDepartment.Sales or EmployeeDepartment.Shipping when item?.Data == null => 2,
                EmployeeDepartment.IT when item==null=>2,
                EmployeeDepartment.IT when item.Data==null=>0,
                null or EmployeeDepartment.Sales or EmployeeDepartment.Shipping or EmployeeDepartment.IT=>0,
                _ => throw new NotImplementedException($"{action.View()} department:{employee.Department}")
            };

        private static int AssertProductReportActionItems(this SingleChoiceAction action, Employee employee)
            => employee?.Department switch{
                null or EmployeeDepartment.Sales=> 4,
                _ => throw new NotImplementedException($"{action.View()} department:{employee.Department}")
            };
        
        private static int AssertCustomerReportActionItems(this SingleChoiceAction action, Employee employee) 
            => employee?.Department switch{
                null => 3, EmployeeDepartment.Sales => 2,
                EmployeeDepartment.Support or EmployeeDepartment.Shipping or EmployeeDepartment.Engineering
                    or EmployeeDepartment.Management or EmployeeDepartment.IT => -1,
                _ => throw new NotImplementedException($"{action.View()} department:{employee.Department}")
            };

        public static bool AssertMapItAction(this SimpleAction action)
            => action.UseCurrentEmployee(employee => employee?.Department switch{ _ => true });

        
        public static IObservable<XafApplication> CanNavigate(this XafApplication application, string viewId) 
            => application.Defer(() => application.NewObjectSpace()
                .Use(space => space.CurrentUser<ApplicationUser>().NavigationViews().Contains(viewId).Observe()).WhenNotDefault()
                .To(application));
        
        public static int NavigationItems(this SingleChoiceAction action, ChoiceActionItem item=null) 
            => action.UseCurrentUser(user => user.IsAdmin() ? item.AdminNavigationItems() : item.EmployeeNavigationItems( user));

        private static int EmployeeNavigationItems(this ChoiceActionItem item, ApplicationUser user){
            if (item == null) return 1;
            var employee = user.Employee();
            return employee.Department switch{
                EmployeeDepartment.Sales => 6,
                EmployeeDepartment.Management => 4,
                EmployeeDepartment.Support or EmployeeDepartment.Shipping or EmployeeDepartment.Engineering
                    or EmployeeDepartment.IT or EmployeeDepartment.HumanResources => 3,
                _ => throw new NotImplementedException(employee.Department.ToString())
            };
        }

        private static int AdminNavigationItems(this ChoiceActionItem item) 
            => item switch{
                null => 3,
                { Caption: "Admin Portal" } => 3,
                { Caption: "Default" } => 7,
                { Caption: "Reports" } => 1,
                _ => throw new NotImplementedException(item.Caption)
            };

        static AssertAction EmployeeTaskAssertAction(this EmployeeDepartment? department, Frame frame) 
            => department switch{
                EmployeeDepartment.Sales when !frame.View.IsRoot=>XAF.Testing.XAF.AssertAction.Process,
                _ => XAF.Testing.XAF.AssertAction.All
            };

        static AssertAction TaskAttachedFileAssertAction(this EmployeeDepartment? department) 
            => department switch{
                _ => XAF.Testing.XAF.AssertAction.HasObject
            };

        static AssertAction CustomerAssertAction(this EmployeeDepartment? department,Frame frame) 
            => department switch{
                EmployeeDepartment.Support or EmployeeDepartment.Shipping or EmployeeDepartment.Engineering
                    or EmployeeDepartment.Management or EmployeeDepartment.IT when frame.View.IsRoot => XAF.Testing.XAF.AssertAction.Process,
                _ => XAF.Testing.XAF.AssertAction.All
            };

        static AssertAction CustomerCommunicationAssertAction(this EmployeeDepartment? department,Frame frame) 
            => department switch{
                EmployeeDepartment.Support or EmployeeDepartment.Shipping or EmployeeDepartment.Engineering
                    or EmployeeDepartment.Management or EmployeeDepartment.IT when !frame.View.IsRoot => XAF.Testing.XAF.AssertAction.Process,
                _=>XAF.Testing.XAF.AssertAction.All
            };

        static AssertAction QuoteItemAssertAction(this EmployeeDepartment? department,Frame frame) 
            => department switch{
                EmployeeDepartment.Shipping or EmployeeDepartment.Engineering or EmployeeDepartment.Management or EmployeeDepartment.IT when
                    !frame.View.IsRoot => XAF.Testing.XAF.AssertAction.NotHasObject,
                _ => XAF.Testing.XAF.AssertAction.All
            };
    
        static AssertAction QuoteAssertAction(this EmployeeDepartment? department,Frame frame) 
            => department switch{
                _ when frame.View.IsRoot && department!=null=> XAF.Testing.XAF.AssertAction.HasObject,
                EmployeeDepartment.Shipping or EmployeeDepartment.Engineering or EmployeeDepartment.Management
                    or EmployeeDepartment.IT when !frame.View.IsRoot => XAF.Testing.XAF.AssertAction.Process,
                _ => XAF.Testing.XAF.AssertAction.All
            };

        static AssertAction CustomerStoreAssertAction(this EmployeeDepartment? department,Frame frame) 
            => department switch{
                EmployeeDepartment.Support or EmployeeDepartment.Shipping or EmployeeDepartment.Engineering
                    or EmployeeDepartment.Management or EmployeeDepartment.IT when !frame.View.IsRoot => XAF.Testing.XAF.AssertAction.Process,
                _ => XAF.Testing.XAF.AssertAction.All
            };

        static AssertAction OrderAssertAction(this EmployeeDepartment? department,Frame frame) 
            => department switch{
                EmployeeDepartment.Support or EmployeeDepartment.Engineering or EmployeeDepartment.Management when !frame.View.IsRoot=>XAF.Testing.XAF.AssertAction.Process,
                _ => XAF.Testing.XAF.AssertAction.All
            };

        static AssertAction OrderItemAssertAction(this EmployeeDepartment? department,Frame frame) 
            => department switch{
                EmployeeDepartment.Support when !frame.View.IsRoot=>XAF.Testing.XAF.AssertAction.Process,
                EmployeeDepartment.Engineering or EmployeeDepartment.Management when !frame.View.IsRoot=>XAF.Testing.XAF.AssertAction.NotHasObject,
                _ when !frame.View.IsRoot&& frame.ParentObject() is Product=> XAF.Testing.XAF.AssertAction.Process,
                _ => XAF.Testing.XAF.AssertAction.All
            };

        static AssertAction CustomerEmployeeAssertAction(this EmployeeDepartment? department,Frame frame) 
            => department switch{
                EmployeeDepartment.Support or EmployeeDepartment.Shipping or EmployeeDepartment.Engineering
                    or EmployeeDepartment.Management or EmployeeDepartment.IT when !frame.View.IsRoot => XAF.Testing.XAF.AssertAction.Process,
                _ => XAF.Testing.XAF.AssertAction.All
            };

        static AssertAction EvaluationAssertAction(this EmployeeDepartment? department, Frame frame) 
            => department switch{
                EmployeeDepartment.Sales when !frame.View.IsRoot=>XAF.Testing.XAF.AssertAction.Process,
                _ => XAF.Testing.XAF.AssertAction.All
            };

        public static AssertAction AssertAction(this Frame frame,Frame source=null){
            var masterFrame = frame.MasterFrame();
            return masterFrame.Assert(masterFrame.View.ObjectTypeInfo.Type,
                masterFrame.View.ObjectSpace.CurrentUser().Employee()?.Department, source);
        }
        static AssertAction EmployeeAssertAction(this EmployeeDepartment? department, Frame frame, Frame source) 
            => department switch {
                EmployeeDepartment.Sales when frame.View.IsRoot=>XAF.Testing.XAF.AssertAction.Process,
                _ when !frame.View.IsRoot && source.View.ObjectTypeInfo.Type == typeof(EmployeeTask) => XAF.Testing.XAF.AssertAction.HasObject,
                _ => XAF.Testing.XAF.AssertAction.AllButDelete,
            };
        private static AssertAction Assert(this Frame frame, Type type, EmployeeDepartment? department,Frame source) 
            => type switch{
                _ when type == typeof(CustomerEmployee) => department.CustomerEmployeeAssertAction(frame),
                _ when type == typeof(OrderItem) => department.OrderItemAssertAction(frame),
                _ when type == typeof(Order) => department.OrderAssertAction(frame),
                _ when type == typeof(CustomerStore) => department.CustomerStoreAssertAction(frame),
                _ when type == typeof(QuoteItem) => department.QuoteItemAssertAction(frame),
                _ when type == typeof(Quote) => department.QuoteAssertAction(frame),
                _ when type == typeof(CustomerCommunication) => department.CustomerCommunicationAssertAction(frame),
                _ when type == typeof(Customer) => department.CustomerAssertAction(frame),
                _ when type == typeof(Employee) => department.EmployeeAssertAction(frame,source),
                _ when type == typeof(Evaluation) => department.EvaluationAssertAction(frame),
                _ when type == typeof(EmployeeTask) => department.EmployeeTaskAssertAction(frame),
                _ when type == typeof(TaskAttachedFile) => department.TaskAttachedFileAssertAction(),
                _ when type == typeof(Product) => department.TaskAttachedFileAssertAction(),
                _ => throw new NotImplementedException($"{frame.View} {department}")
            };
    }
}