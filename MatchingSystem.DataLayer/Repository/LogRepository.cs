using Dapper;
using MatchingSystem.DataLayer.Base;
using MatchingSystem.DataLayer.Interface;
using System.Data.SqlClient;

namespace MatchingSystem.DataLayer.Repository
{
    public class LogRepository : ConnectionBase, ILoggerRepository
    {
        public LogRepository(string connectionString) : base(connectionString)
        {
        }

        public void LogRequest(string request, string endpoint)
        {
            using var db = new SqlConnection(ConnectionString);
            var result = db.Execute("INSERT INTO [dbo].[Log] (Request, Endpoint, RequestDate) VALUES (@request, @endpoint, GETDATE())", 
                    new { request, endpoint }
            );
        }
    }
}
