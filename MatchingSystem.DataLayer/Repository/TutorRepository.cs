using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Base;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Dto.MatchingMonitoring;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MatchingSystem.DataLayer.Repository
{
    public class TutorRepository : ConnectionBase, ITutorRepository
    {
        public TutorRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<bool> GetReadyByTutorAsync(int tutorId)
        {
            return await Connection.ExecuteScalarAsync<bool>(
                "select napp.get_IsReadyToStart_ByTutor (@TutorID)",
                new { TutorID = tutorId }
            );
        }

        public bool GetReadyByTutor(int tutorId)
        {
            return Connection.ExecuteScalar<bool>(
                "select napp.get_IsReadyToStart_ByTutor (@TutorID)",
                new { TutorID = tutorId }
            );
        }

        public async Task<int> GetTutorIdAsync(int userId, int matchingId)
        {
            return await Connection.ExecuteScalarAsync<int>(
                "select napp.get_TutorID(@UserID, @MatchingID)",
                new { UserID = userId, MatchingID = matchingId }
            );
        }
        
        public IEnumerable<TutorFullDTO> GetFullInfoTutorByMatching(int matchingId)
        {
            return Connection.Query<TutorFullDTO>(
                "select tut.TutorID,tut.NameAbbreviation,tut.UserID,TUT.MatchingID,TUT.IsClosed, tut.LastVisitDate,Qty from dbo_v.Tutors tut " +
            "join CommonQuotas cq on cq.TutorID = tut.TutorID " +
            "where QuotaStateID = 1 AND MatchingID = @MatchingID "
                , new {@MatchingID = matchingId }
            );
        }

        public int GetTutorId(int userId, int matchingId)
        {
            return Connection.ExecuteScalar<int>(
                "select napp.get_TutorID(@UserID, @MatchingID)",
                new { UserID = userId, MatchingID = matchingId }
            );
        }

        public async Task<IEnumerable<Tutor>> GetTutorsByMatchingAsync(int matchingId)
        {
            return await Connection.QueryAsync<Tutor>(
                "select TutorID, TutorNameAbbreviation from napp.get_Tutors_ByMatching(@MatchingID)",
                new { MatchingID = matchingId }
            );
        }

        public void setNewUserRoles_Tutors(List<TutorInitDto> tuts,int matchingID)
        {
            foreach (var tut in tuts)
            {
                Connection.ExecuteAsync(
                    "insert Users_Roles (UserID,RoleID,MatchingID,TutorID) VALUES (@UserID,@RoleID,@MatchingID,@TutorID)", new
                    {
                        UserID = tut.UserId
                        ,RoleID = 1
                        ,MatchingID = matchingID
                        ,TutorID = tut.TutorId
                    });
            }
        }
        
        public IEnumerable<TutorInitDto> SetNewTutors(List<TutorInitDto> tuts,int matchingId)
        {
            foreach (var tut in tuts)
            {
                tut.TutorId = Connection.ExecuteScalar<int>(
                    "insert Tutors (MatchingID) OUTPUT INSERTED.TutorID VALUES (@MatchingID)", new
                    {
                        MatchingID = matchingId
                    });
            }
            return tuts;
        }

        public void SetCommonQuotasForTutors(List<TutorInitDto> tuts,int stageId)
        {
            foreach (var tut in tuts)
            {

                Connection.Execute("INSERT INTO CommonQuotas(TutorID,Qty,CreateDate,QuotaStateId,StageId) VALUES(@TutorID,@Qty,getdate(),1,@StageId)"
                    , new { 
                        TutorID = tut.TutorId
                        ,@Qty = tut.quota
                        ,@StageId = stageId

                    });   
            }
        }
        
        public IEnumerable<Tutor> GetTutorsByMatching(int matchingId)
        {
            return Connection.Query<Tutor>(
                "select TutorID, TutorNameAbbreviation from napp.get_Tutors_ByMatching(@MatchingID)",
                new { MatchingID = matchingId }
            );
        }

        public async Task<IEnumerable<Group>> GetGroupsByTutorAsync(int tutorId)
        {
            return await Connection.QueryAsync<Group>(
                "select * from napp.get_Groups_ByTutor(@TutorID)",
                new { TutorID = tutorId }
            );
        }

        public IEnumerable<Group> GetGroupsByTutor(int tutorId)
        {
            return Connection.Query<Group>(
                "select * from napp.get_Groups_ByTutor(@TutorID)",
                new { TutorID = tutorId }
            );
        }

        public async Task<IEnumerable<TutorChoice>> GetChoiceByTutorAsync(int tutorId)
        {
            return await Connection.QueryAsync<TutorChoice>(
                "select ChoiceID, " +
                "StudentNameAbbreviation, " +
                "GroupName, " +
                "ProjectID, " +
                "StudentID, " +
                "ProjectName, " +
                "IsInQuota, " +
                "Qty, " +
                "SortOrderNumber, " +
                "ProjectIsClosed, " +
                "UpdateDate" +
                "TypeCode " +
                "from napp.get_TutorsChoice_ByTutor(@TutorID)",
                new { TutorID = tutorId }
            );
        }

        public IEnumerable<TutorChoice> GetChoiceByTutor(int tutorId)
        {
            return Connection.Query<TutorChoice>(
                "select ChoiceID, " +
                "StudentNameAbbreviation, " +
                "GroupName, " +
                "ProjectID, " +
                "StudentID, " +
                "ProjectName, " +
                "IsInQuota, " +
                "Qty, " +
                "SortOrderNumber, " +
                "ProjectIsClosed, " +
                "UpdateDate," +
                "TypeCode " +
                "from napp.get_TutorsChoice_ByTutor(@TutorID)",
                new { TutorID = tutorId }
            );
        }

        public IEnumerable<TutorChoice> getChoicesByMatchingCurrentStage(int MatchingId)
        {
            return Connection.Query<TutorChoice>(
                "SELECT " +
                "ChoiceID" +
                ",StudentID" +
                ",null" +
                ",null" +
                ",ProjectID" +
                ",null" +
                ",IsInQuota" +
                ",null" +
                ",SortOrderNumber " +
                ",null" +
                ",UpdateDate" +
                ",TypeID " +
                "from dbo.TutorsChoice Choice with(nolock) " +
                "WHERE " +
                "Choice.StageID = napp_in.get_CurrentStageID_ByMatching(@MatchingId)",
                new { MatchingId = MatchingId }
               );
        }

        public IEnumerable<TutorChoice> getChoicesByMatching(int MatchingId)
        {
            return Connection.Query<TutorChoice>(
               "SELECT ChoiceID " +
               ",StudentID	" +
               ",null--StudentNameAbbreviation	" +
               ",null--GroupName	" +
               ",ProjectID	" +
               ",null--ProjectName" +
               ",IsInQuota " +
               ",null--Qty " +
               ",SortOrderNumber " +
               ",null--ProjectIsClosed " +
               "UpdateDate" +
               ",TypeID " +
               "from dbo.TutorsChoice Choice with(nolock) " +
               "WHERE " +
               "Choice.StageID in (select StageID from stages where MatchingId = @MatchingId))",
               new { MatchingId = MatchingId }
               );
        }


        public async Task<int> GetCommonQuotaByTutorAsync(int tutorId)
        {
            return await Connection.ExecuteScalarAsync<int>(
                "select napp.get_CommonQuota_ByTutor(@TutorID)", 
                new { TutorID = tutorId }
            );
        }

        public int GetCommonQuotaByTutor(int tutorId)
        {
            return Connection.ExecuteScalar<int>(
                "select napp.get_CommonQuota_ByTutor(@TutorID)", 
                new { TutorID = tutorId }
            );
        }

        public async Task<IEnumerable<QuotaHistoryTutor>> GetQuotaRequestHistoryByTutorAsync(int tutorId)
        {
            return await Connection.QueryAsync<QuotaHistoryTutor>(
                "select CreateDate, Qty, StageTypeName_ru, QuotaStateName_ru, QuotaStateCode, IterationNumber " +
                "from napp.get_CommonQuota_History_ByTutor(@TutorID) order by CreateDate desc",
                new { tutorId }
            );
        }

        public IEnumerable<QuotaHistoryTutor> GetQuotaRequestHistoryByTutor(int tutorId)
        {
            return Connection.Query<QuotaHistoryTutor>(
                "select CreateDate, Qty, StageTypeName_ru, QuotaStateName_ru, QuotaStateCode, IterationNumber " +
                "from napp.get_CommonQuota_History_ByTutor(@TutorID) order by CreateDate desc",
                new { tutorId }
            );
        }

        public async Task CreateCommonQuotaRequestForSecondStageAsync(int tutorId, int quota, string message)
        {
            await Connection.ExecuteAsync(
                "exec napp.create_CommonQuota_Request @TutorID, @NewQuotaQty, @Message",
                new { TutorID = tutorId, NewQuotaQty = quota, Message = message }
            );
        }

        public void CreateCommonQuotaRequestForSecondStage(int tutorId, int quota, string message)
        {
            Connection.Execute(
                "exec napp.create_CommonQuota_Request @TutorID, @NewQuotaQty, @Message",
                new { TutorID = tutorId, NewQuotaQty = quota, Message = message }
            );
        }

        public async Task CreateCommonQuotaRequestForThirdStageAsync(int tutorId, int quota, string message)
        {
            await Connection.ExecuteAsync(
                "exec napp.create_CommonQuota_Request @TutorID, @NewQuotaQty, @Message",
                new { TutorID = tutorId, NewQuotaQty = quota, Message = message }
            );
        }

        public void CreateCommonQuotaRequestForThirdStage(int tutorId, int quota, string message)
        {
            Connection.Execute(
                "exec napp.create_CommonQuota_Request @TutorID, @NewQuotaQty, @Message",
                new { TutorID = tutorId, NewQuotaQty = quota, Message = message }
            );
        }

        public async Task CreateCommonQuotaRequestForLastStageAsync(CreateCommonQuotaParams @params)
        {
            //TODO потенциально опасное место из-за того что не указан тип передаваемой таблицы, но мб для Dapper это и не нужно
            await Connection.ExecuteAsync(
                "exec napp.create_CommonQuota_Request @TutorID, @NewQuotaQty, @Message, @ProjectQuota",
                new
                {
                    TutorID = @params.TutorId,
                    NewQuotaQty = @params.NewQuota,
                    Message = @params.Message,
                    ProjectQuota = @params.Table
                }
            );
        }

        public void CreateCommonQuotaRequestForLastStage(CreateCommonQuotaParams @params)
        {
            //TODO потенциально опасное место из-за того что не указан тип передаваемой таблицы, но мб для Dapper это и не нужно
            Connection.Execute(
                "exec napp.create_CommonQuota_Request @TutorID, @NewQuotaQty, @Message, @ProjectQuota",
                new
                {
                    TutorID = @params.TutorId,
                    NewQuotaQty = @params.NewQuota,
                    Message = @params.Message,
                    ProjectQuota = @params.Table.AsTableValuedParameter("[dbo].[ProjectQuota]")
                }
            );
        }

        public async Task<int> GetNotificationsCountByTutorAsync(int tutorId)
        {
            return await Connection.ExecuteScalarAsync<int>(
                "select count(*) from napp.get_CommonQuota_Request_Notification_ByTutor(@TutorID)",
                new { TutorID = tutorId }
            );
        }

        public int GetNotificationsCountByTutor(int tutorId)
        {
            return Connection.ExecuteScalar<int>(
                "select count(*) from napp.get_CommonQuota_Request_Notification_ByTutor(@TutorID)",
                new { TutorID = tutorId }
            );
        }

        public int GetCurrentMatching(int tutorId)
        {
            throw new System.NotImplementedException();
        }

        public async Task SetReadyAsync(int tutorId)
        {
            await Connection.ExecuteAsync(
                "exec napp.upd_Tutor_IsReadyToStart @TutorID, @IsReady",
                new { TutorID = tutorId, IsReady = true }
            );
        }

        public void SetReady(int tutorId)
        {
            Connection.Execute(
                "exec napp.upd_Tutor_IsReadyToStart @TutorID, @IsReady",
                new { TutorID = tutorId, IsReady = true }
            );
        }

        public void SetPreferences(IEnumerable<TutorChoice_1> choices, int tutorId)
        {
            var table = new DataTable();
            table.Columns.Add("ChoiceID", typeof(int));
            table.Columns.Add("SortOrderNumber", typeof(short));
            table.Columns.Add("IsInQuota", typeof(bool));

            foreach (var item in choices)
            {
                var row = table.NewRow();
                row.SetField("ChoiceID", item.ChoiceID);
                row.SetField("SortOrderNumber", item.SortOrderNumber);
                row.SetField("IsInQuota", item.IsInQuota);
                table.Rows.Add(row);
            }

            Connection.Execute(
                "exec napp.upd_TutorsChoice @Choices, @TutorID",
                new {Choices = table.AsTableValuedParameter("dbo.TutorsChoice_1"), TutorID = tutorId}
            );
        }

        public async Task SetPreferencesAsync(IEnumerable<TutorChoice_1> choices, int tutorId)
        {
            var table = new DataTable();
            table.Columns.Add("ChoiceID", typeof(int));
            table.Columns.Add("SortOrderNumber", typeof(short));
            table.Columns.Add("IsInQuota", typeof(bool));

            foreach (var item in choices)
            {
                var row = table.NewRow();
                row.SetField("ChoiceID", item.ChoiceID);
                row.SetField("SortOrderNumber", item.SortOrderNumber);
                row.SetField("IsInQuota", item.IsInQuota);
                table.Rows.Add(row);
            }

            await Connection.ExecuteAsync(
                "exec napp.upd_TutorsChoice @Choices, @TutorID",
                new {Choices = table.AsTableValuedParameter("dbo.TutorsChoice_1"), TutorID = tutorId}
            );
        }
        public IEnumerable<Tutor> GetAllTutors()
        {
            return Connection.Query<Tutor>(
                "select distinct NameAbbreviation, UserID from dbo_v.Tutors" 
            );
        }
    }
}