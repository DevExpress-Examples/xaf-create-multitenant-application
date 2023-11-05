using System.IO.Compression;
using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using Microsoft.Data.SqlClient;
using OutlookInspired.Module.BusinessObjects;
using OutlookInspired.Module.Features.MasterDetail;
using OutlookInspired.Module.Services.Internal;

namespace OutlookInspired.Module.Services{
    public static class Extensions{
        public static void AttachDatabase(this SqlConnectionStringBuilder builder,string dataPath){
            var initialCatalog = "Initial catalog";
            var databaseName = builder[initialCatalog].ToString();
            builder.Remove(initialCatalog);
            using var sqlConnection = new SqlConnection(builder.ConnectionString);
            sqlConnection.Open();
            using var command = new SqlCommand();
            command.Connection = sqlConnection;
            var fullDataPath = Path.GetFullPath(dataPath);
            var userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var destFileName = $"{userProfilePath}\\{databaseName}.mdf";
            if (!File.Exists(destFileName)) {
                ZipFile.ExtractToDirectory($"{fullDataPath}\\OutlookInspired.zip", userProfilePath);
                File.Move($"{userProfilePath}\\OutlookInspired.mdf", destFileName);
            }
            command.CommandText = $@"
                        IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{databaseName}')
                        BEGIN
                            CREATE DATABASE {databaseName} ON (FILENAME = '{destFileName}') FOR ATTACH_REBUILD_LOG;
                        END";
            command.ExecuteNonQuery();
        }

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