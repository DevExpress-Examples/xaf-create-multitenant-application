using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.MasterDetail;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Module.Services{
    public static class Extensions{
        
        public static IEnumerable<IUserControl> FilterUserControl(this DetailView view, LambdaExpression expression) 
            => view.UserControl().YieldItem().WhereNotDefault()
                .Where(control => control.ObjectType == expression.Parameters.First().Type)
                .Do(control => control.SetCriteria(expression)).ToArray();
        
        public static T UseCurrentEmployee<T>(this ActionBase action,Func<Employee,T> withEmployee){
            var view = action.View();
            var applicationUser = view!=null? view.ObjectSpace.CurrentUser():action.Application.NewObjectSpace().CurrentUser();
            var result = withEmployee(applicationUser.Employee());
            if (view == null){
                ((IObjectSpaceLink)applicationUser).ObjectSpace.Dispose();
            }
            return result;
        }
        public static T UseCurrentUser<T>(this ActionBase action,Func<ApplicationUser, T> withUser){
            var view = action.View();
            var applicationUser = view!=null? view.ObjectSpace.CurrentUser():action.Application.NewObjectSpace().CurrentUser();
            var result = withUser(applicationUser);
            if (view == null){
                ((IObjectSpaceLink)applicationUser).ObjectSpace.Dispose();
            }
            return result;
        }

        public static Employee Employee(this ApplicationUser applicationUser) 
            => ((IObjectSpaceLink)applicationUser).ObjectSpace.GetObjectsQuery<Employee>().FirstOrDefault(employee => employee.User.ID == applicationUser.ID);
        
        public static ApplicationUser CurrentUser(this IObjectSpace objectSpace) 
            => objectSpace.CurrentUser<ApplicationUser>();
    }
}