using Microsoft.Data.SqlClient;

namespace MatchingSystem.DataLayer.Base
{
    public class ConnectionBase
    {
        protected readonly string ConnectionString;
        protected SqlConnection Connection;
        protected ConnectionBase(string connectionString)
        {
            ConnectionString = connectionString;
            Connection = new SqlConnection(ConnectionString);
        }
    }
}