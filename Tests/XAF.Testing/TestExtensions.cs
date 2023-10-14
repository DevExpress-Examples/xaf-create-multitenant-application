using System.Data.SqlClient;
using Aqua.EnumerableExtensions;
using DevExpress.ExpressApp;
using XAF.Testing.XAF;

namespace XAF.Testing{
    public static class TestExtensions{
        public static void DeleteModelDiffs(this XafApplication application,string connectionString,string modelDifferenceTableName,string modelDifferenceAspectTableName){
            application.ConnectionString=connectionString;
            if (application.DbExist()){
                using var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                using var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText=new[]{modelDifferenceTableName,modelDifferenceAspectTableName}
                    .Select(table => $"IF OBJECT_ID('{table}', 'U') IS NOT NULL Delete FROM {table};").StringJoin("");
                sqlCommand.ExecuteNonQuery();    
            }
        }

    }
}