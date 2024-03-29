﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Base;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;

namespace MatchingSystem.DataLayer.Repository
{
    public class MatchingRepository : ConnectionBase, IMatchingRepository
    {
        public MatchingRepository(string connectionString) : base(connectionString)
        {
        }

        public Stage GetCurrentStage(int matchingId)
        {
            return Connection.QueryFirstOrDefault<Stage>(
                "select * from napp.get_CurrentStage_ByMatching(@MatchingID)",
                new {MatchingID = matchingId});
        }

        public IEnumerable<Matching> GetMatchings()
        {
            return GetMatchingsAsync().Result;
        }

        public async Task<IEnumerable<Matching>> GetMatchingsAsync()
        {
            return await Connection.QueryAsync<Matching>("select * from dbo.Matching");
        }

        public async Task<Stage> GetCurrentStageAsync(int matchingId)
        {
            return await Connection.QueryFirstOrDefaultAsync<Stage>(
                "select * from napp.get_CurrentStage_ByMatching(@MatchingID)",
                new {MatchingID = matchingId});
        }

        public IEnumerable<Allocation> GetFinalAllocations()
        {
            return Connection.Query<Allocation>("select MatchingID, " +
                                                "ProjectID, StudentID, TutorID, GroupID, " +
                                                "StudentNameAbbreviation, GroupName, " +
                                                "ProjectName, TutorNameAbbreviation, IsAllocated " +
                                                "from napp.get_FinishedAllocations()");
        }

        public IEnumerable<Allocation> GetFinalAllocationByMatching(int MatchingId)
        {
            return Connection.Query<Allocation>("SELECT " +
                                                "MatchingID, " +
                                                "ProjectID, StudentID, TutorID, GroupID, " +
                                                "StudentNameAbbreviation, GroupName, " +
                                                "ProjectName, TutorNameAbbreviation, IsAllocated " +
                                                "from napp.get_FinishedAllocations() " +
                                                "WHERE MatchingId = @MatchingId",
                                                new {MatchingId = MatchingId }
                                                );
        }

        public async Task<IEnumerable<Allocation>> GetFinalAllocationsAsync()
        {
            return await Connection.QueryAsync<Allocation>("select MatchingID, " +
                                                           "ProjectID, StudentID, TutorID, GroupID, " +
                                                           "StudentNameAbbreviation, GroupName, " +
                                                           "ProjectName, TutorNameAbbreviation, IsAllocated " +
                                                           "from napp.get_FinishedAllocations()");
        }

        public IEnumerable<Matching> GetMatchingsByUser(int userId)
        {
            return Connection.Query<Matching>("select MatchingID, MatchingName, MatchingTypeName_ru " +
                                              "from napp.get_UserMatchings(@UserID, null)"
                , new {UserID = userId});
        }

        public async Task<IEnumerable<Matching>> GetMatchingsByUserAsync(int userId)
        {
            return await Connection.QueryAsync<Matching>("select MatchingID, MatchingName, MatchingTypeName_ru " +
                                                         "from napp.get_UserMatchings(@UserID, null)"
                , new {UserID = userId});
        }

        public IEnumerable<MatchingInfo> GetMatchingsInfo()
        {
            return Connection.Query<MatchingInfo>("select * from napp.get_FinishedMatchings()");
        }

        public  int CreateMatching(MatchingInitDto data)
        {
            return Connection.ExecuteScalar<int>("insert into Matching (MatchingName,MatchingTypeID,CreatorUserID) output INSERTED.MatchingID Values(@MatchingName,@MatchingTypeId,@CreatorUserId)"
                ,new {
                    MatchingName = data.name,
                    MatchingTypeId = (data.typeCode=="MG")?2:1,
                    CreatorUserId=2
                });
        }

        public int SetNewFirstStageInMatching(int MatchingID)
        {
            var IdInsertedRecord = Connection.ExecuteScalar<int>("insert into Stages(StageTypeID,StartDate,EndPlanDate,IsCurrent,MatchingID) OUTPUT INSERTED.StageID VALUES(1,getdate(),DATEADD(HOUR,1,getdate()),1,@MatchingID)"
                ,new {
                    MatchingID = MatchingID
                }
            );
            return IdInsertedRecord;
        }
        
        public async Task<IEnumerable<MatchingInfo>> GetMatchingsInfoAsync()
        {
            return await Connection.QueryAsync<MatchingInfo>("select * from napp.get_FinishedMatchings()");
        }

        public void SetNextStage(int matchingId)
        {
            Connection.Query("exec napp.goto_NextStage @MatchingID",
                new {MatchingID = matchingId});
        }

        public async Task SetNextStageAsync(int matchingId)
        {
            await Connection.QueryAsync("exec napp.goto_NextStage @MatchingID",
                new {MatchingID = matchingId});
        }

        public void SetStageEndDate(DateTime date, int matchingId)
        {
            Connection.Query("exec napp.upd_CurrentStage_EndPlanDate @MatchingID, @EndDate",
                new {MatchingID = matchingId, EndDate = date});
        }

        public async Task SetStageEndDateAsync(DateTime date, int matchingId)
        {
            await Connection.QueryAsync("exec napp.upd_CurrentStage_EndPlanDate @MatchingID, @EndDate",
                new {MatchingID = matchingId, EndDate = date});
        }
    }
}