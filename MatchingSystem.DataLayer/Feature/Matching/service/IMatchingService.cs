using System;
using MatchingSystem.DataLayer.Dto.MatchingInit;

namespace MatchingSystem.DataLayer.Feature.Matching.service;

public interface IMatchingService 
{
       
    void SetStageEndDate(DateTime date, int matchingId);
    void SetNextStage(int matchingId);
    int CreateMatching(MatchingInitDto data);
    int SetNewFirstStageInMatching(int MatchingID);
}