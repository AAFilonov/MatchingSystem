namespace MatchingSystem.DataLayer.Feature.Matching.service;

public interface IMatchingRepositoryDapper
{
    void SetNextStage(int matchingId);
}