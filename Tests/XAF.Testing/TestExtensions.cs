using System.Data.SqlClient;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.MultiTenancy;
using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.EntityFrameworkCore;
using XAF.Testing.XAF;

namespace XAF.Testing{
    public static class TestExtensions{
        public static IObservable<XafApplication> DeleteModelDiffs<T>(this IObservable<XafApplication> source,Func<XafApplication,string> connectionString,string user=null) where T : DbContext 
            => source.Do(application => {
                application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
                var tenantProvider = application.GetService<ITenantProvider>();
                if (tenantProvider == null){
                    application.DeleteModelDiffs<T>(connectionString(application));
                }
                else{
                    using var sqlConnection = new SqlConnection(connectionString(application));
                    var tenant = sqlConnection.GetTenant(user);
                    tenantProvider.TenantId = tenant.id;
                    application.DeleteModelDiffs<T>(tenant.connectionString);
                }
            });

        public static (Guid id, string connectionString) GetTenant(this SqlConnection connection, string user){
            var query = "SELECT ID, ConnectionString FROM Tenant WHERE Name = @Name";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", user.Split('@')[1]);
            connection.Open();
            using var reader = command.ExecuteReader();
            return reader.Read() ? (reader.GetGuid(0), reader.GetString(1))
                : throw new InvalidOperationException("Tenant not found.");
        }
        
        public static void DeleteModelDiffs<TDBContext>(this XafApplication application,string connectionString=null) where TDBContext:DbContext{
            connectionString ??= application.ConnectionString;
            if (!application.DbExist(connectionString)) return;
            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            using var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText=new []{typeof(ModelDifference),typeof(ModelDifferenceAspect)}
                .SelectMany(type => application.GetRequiredService<TDBContext>().Model.FindEntityTypes(type)
                    .Select(entityType => entityType.GetTableName()))
                .Select(table => $"IF OBJECT_ID('{table}', 'U') IS NOT NULL Delete FROM {table};").StringJoin("");
            sqlCommand.ExecuteNonQuery();
        }

        public static Exception ToTestException(this Exception exception,[CallerMemberName]string caller="") => TestException.New(exception,caller);

        public static IObservable<Exception> ThrowTestException(this Exception exception,[CallerMemberName]string caller="")
            => exception.Observe().ThrowTestException(caller);
        
        public static IObservable<Exception> ThrowTestException(this IObservable<Exception> source,[CallerMemberName]string caller="") 
            => source.If(exception => exception is not TestException,exception => exception.ToTestException(caller).Observe(),
                    exception => exception.Observe()).Cast<TestException>().SelectMany(Observable.Throw<Exception>);
        
    }
    
    sealed class TestException : Exception{
        private TestException(Exception exception,[CallerMemberName]string caller="") : this(
            $"{exception.GetType().Name} - {caller} - {exception.Message}{Environment.NewLine}{Environment.NewLine}{ScreenCapture.CaptureActiveWindowAndSave()}.{Environment.NewLine}",
            $"{Environment.NewLine}{exception.ReverseStackTrace()}"){
        }

        public static TestException New(Exception exception,[CallerMemberName]string caller="") => new(exception,caller);

        private TestException(string message, string stackTrace) : base(message) => Data["StackTrace"] = stackTrace;

        public override string StackTrace => Data["StackTrace"] as string;
    }
}