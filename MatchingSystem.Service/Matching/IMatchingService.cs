using System;
using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.OldDto.MatchingInit;

namespace MatchingSystem.Service.Matching;

public interface IMatchingService 
{
       
    void SetStageEndDate(DateTime date, int matchingId);
    void SetNextStage(int matchingId);
    int CreateMatching(MatchingInitDto data);
    int createFirstStage(int MatchingID);
    IEnumerable<MatchingDto> GetMatchingsByUser(int userId);
}