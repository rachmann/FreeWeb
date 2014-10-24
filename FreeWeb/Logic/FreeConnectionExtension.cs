using System.Configuration;
using System.Data.SqlClient;

namespace FreeWeb.Logic
{
    public static class FreeConnectionExtension
    {
        public static SqlConnection FreeConnection(this ConnectionStringSettings settings)
        {
            var dbConnection = new SqlConnection(settings.ConnectionString);
            dbConnection.Open();
            return dbConnection;
        }


    }
}