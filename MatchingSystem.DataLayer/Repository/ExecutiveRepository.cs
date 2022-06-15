using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Base;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.IO.Params;

namespace MatchingSystem.DataLayer.Repository
{
    public class ExecutiveRepository : ConnectionBase, Interface.IExecutiveRepository
    {
        public ExecutiveRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SetAdjustmentByExecutiveAsync(AdjustmentParams @params)
        {
            await Connection.QueryAsync(
                "exec napp.create_ExecutiveChoice @UserID, @MatchingID, @ProjectID, @StudentID",
                new
                {
                    UserID = @params.UserId,
                    MatchingID = @params.MatchingId,
                    ProjectID = @params.ProjectId,
                    StudentID = @params.StudentId
                });
        }
        public void SetAdjustmentByExecutive(AdjustmentParams @params)
        {
            Connection.Query(
                "exec napp.create_ExecutiveChoice @UserID, @MatchingID, @ProjectID, @StudentID",
                new
                {
                    UserID = @params.UserId,
                    MatchingID = @params.MatchingId,
                    ProjectID = @params.ProjectId,
                    StudentID = @params.StudentId
                });
        }

        public async Task<IEnumerable<Allocation>> GetAllocationsByExecutiveAsync(int userId, int matchingId)
        {
            return await Connection.QueryAsync<Allocation>(
                "select * from napp.get_Allocation_ByExecutive(@UserID, @MatchingID)",
                new {UserID = userId, MatchingID = matchingId});
        }
        public IEnumerable<Allocation>  GetAllocationsByExecutive(int userId, int matchingId)
        {
            return Connection.Query<Allocation>("select * from napp.get_Allocation_ByExecutive(@UserID, @MatchingID)",
                new {UserID = userId, MatchingID = matchingId});
        }


        public async Task<int> GetNotificationsCountByExecutiveAsync(int userId, int matchingId)
        {
            return await Connection.ExecuteScalarAsync<int>("select " +
                                                            "napp.get_CommonQuota_Requests_NotificationCount_ByExecutive(@UserID, @MatchingID) as Value",
                new {UserID = userId, MatchingID = matchingId});
        }
        public int GetNotificationsCountByExecutive(int userId, int matchingId)
        {
            return Connection.ExecuteScalar<int>("select " +
                                                 "napp.get_CommonQuota_Requests_NotificationCount_ByExecutive(@UserID, @MatchingID) as Value",
                new {UserID = userId, MatchingID = matchingId});
        }


        public async Task<IEnumerable<QuotaHistoryExecutive>> GetQuotaRequestHistoryByExecutiveAsync(int userId, int matchingId)
        {
            return await Connection.QueryAsync<QuotaHistoryExecutive>(
                "select * from napp.get_CommonQuota_History_ByExecutive(@UserID, @MatchingID)",
                new {UserID = userId, MatchingID = matchingId});
        }
        public IEnumerable<QuotaHistoryExecutive> GetQuotaRequestHistoryByExecutive(int userId, int matchingId)
        {
            return Connection.Query<QuotaHistoryExecutive>(
                "select * from napp.get_CommonQuota_History_ByExecutive(@UserID, @MatchingID) order by CreateDate desc",
                new {UserID = userId, MatchingID = matchingId});
        }


        public async Task<IEnumerable<QuotaRequest>> GetQuotaRequestsByExecutiveAsync(int userId, int matchingId)
        {
            return await Connection.QueryAsync<QuotaRequest>(
                "select * from napp.get_CommonQuota_Requests_ByExecutive(@UserID, @MatchingID)",
                new {UserID = userId, MatchingID = matchingId});
        }
        public IEnumerable<QuotaRequest> GetQuotaRequestsByExecutive(int userId, int matchingId)
        {
            return Connection.Query<QuotaRequest>(
                "select * from napp.get_CommonQuota_Requests_ByExecutive(@UserID, @MatchingID)",
                new {UserID = userId, MatchingID = matchingId});
        }

       

        public async Task AcceptQuotaRequestAsync(int quotaId, bool result)
        {
            await Connection.ExecuteAsync("exec napp.upd_CommonQuota_Request @QuotaID, @RequestResult",
                new {QuotaID = quotaId, RequestResult = result}
            );
        }
        public void AcceptQuotaRequest(int quotaId, bool result)
        {
            Connection.Execute("exec napp.upd_CommonQuota_Request @QuotaID, @RequestResult",
                new {QuotaID = quotaId, RequestResult = result}
            );
        }
    }
}
