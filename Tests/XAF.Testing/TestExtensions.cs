using System.Data.SqlClient;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.EntityFrameworkCore;
using XAF.Testing.XAF;

namespace XAF.Testing{
    public static class TestExtensions{
        public static void DeleteModelDiffs<TDBContext>(this XafApplication application) where TDBContext:DbContext{
            if (!application.DbExist()) return;
            using var sqlConnection = new SqlConnection(application.ConnectionString);
            sqlConnection.Open();
            using var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText=new []{typeof(ModelDifference),typeof(ModelDifferenceAspect)}
                .SelectMany(type => application.GetRequiredService<TDBContext>().Model.FindEntityTypes(type).Select(entityType => entityType.GetTableName()))
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