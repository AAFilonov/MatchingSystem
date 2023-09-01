using System.Data.SqlClient;

namespace MatchingSystem.DataLayer.Context
{
    public class ConnectionBase
    {
        protected readonly string ConnectionString;
        protected readonly SqlConnection Connection;
        protected ConnectionBase(string connectionString)
        {
            ConnectionString = connectionString;
            Connection = new SqlConnection(ConnectionString);
        }
    }
}