using System.Data.SqlClient;
using Dapper;
using MatchingSystem.DataLayer.Context;
using MatchingSystem.DataLayer.Feature.Interface;

namespace MatchingSystem.DataLayer.Feature.Repository
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
