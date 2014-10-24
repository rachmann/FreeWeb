using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace FreeModels
{
    public class Db
    {
        public IDbConnection GetOpenConnection(string connection)
        {
            var dbConnection = new SqlConnection(connection);
            dbConnection.Open();
            return dbConnection;
        }

    }
}