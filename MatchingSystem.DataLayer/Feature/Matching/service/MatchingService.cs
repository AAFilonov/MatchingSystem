using System;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature.Stage;

namespace MatchingSystem.DataLayer.Feature.Matching.service;

public class MatchingService: IMatchingService
{
    private readonly IMatchingRepository matchingRepository;
    private readonly IStageRepository stageRepository;

    public MatchingService(IMatchingRepository matchingRepository, IStageRepository stageRepository)
    {
        this.matchingRepository = matchingRepository;
        this.stageRepository = stageRepository;
    }


    public void SetStageEndDate(DateTime date, int matchingId)
    {
        var currentStage = matchingRepository.GetCurrentStage(matchingId);
        currentStage.EndDate = date;
        stageRepository.save(currentStage);
    }

    public void SetNextStage(int matchingId)
    {
        throw new NotImplementedException();
    }

    public int CreateMatching(MatchingInitDto data)
    {
        throw new NotImplementedException();
    }

    public int SetNewFirstStageInMatching(int MatchingID)
    {
        throw new NotImplementedException();
    }
}