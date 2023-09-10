using System;
using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.MatchingInit;

namespace MatchingSystem.DataLayer.Feature.Matching.service;

public interface IMatchingService 
{
       
    void SetStageEndDate(DateTime date, int matchingId);
    void SetNextStage(int matchingId);
    int CreateMatching(MatchingInitDto data);
    int createFirstStage(int MatchingID);
    IEnumerable<MatchingDto> GetMatchingsByUser(int userId);
}