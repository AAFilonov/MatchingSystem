using System.Collections.Generic;
using Dapper;
using MatchingSystem.DataLayer.Context;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.IO.Params;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Executive
{
    public class ExecutiveServiceDapper : ConnectionBase, IExecutiveServiceOld
    {
        public ExecutiveServiceDapper(string connectionString) : base(connectionString)
        {
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
        
        public IEnumerable<AllocationView>  GetAllocationsByExecutive(int userId, int matchingId)
        {
            return Connection.Query<AllocationView>("select * from napp.get_Allocation_ByExecutive(@UserID, @MatchingID)",
                new {UserID = userId, MatchingID = matchingId});
        }


        public int GetNotificationsCountByExecutive(int userId, int matchingId)
        {
            return Connection.ExecuteScalar<int>("select " +
                                                 "napp.get_CommonQuota_Requests_NotificationCount_ByExecutive(@UserID, @MatchingID) as Value",
                new {UserID = userId, MatchingID = matchingId});
        }



        public IEnumerable<QuotaHistoryExecutive> GetQuotaRequestHistoryByExecutive(int userId, int matchingId)
        {
            return Connection.Query<QuotaHistoryExecutive>(
                "select * from napp.get_CommonQuota_History_ByExecutive(@UserID, @MatchingID) order by CreateDate desc",
                new {UserID = userId, MatchingID = matchingId});
        }



        public IEnumerable<QuotaRequest> GetQuotaRequestsByExecutive(int userId, int matchingId)
        {
            return Connection.Query<QuotaRequest>(
                "select * from napp.get_CommonQuota_Requests_ByExecutive(@UserID, @MatchingID)",
                new {UserID = userId, MatchingID = matchingId});
        }
        
        public void AcceptQuotaRequest(int quotaId, bool result)
        {
            Connection.Execute("exec napp.upd_CommonQuota_Request @QuotaID, @RequestResult",
                new {QuotaID = quotaId, RequestResult = result}
            );
        }
    }
}
