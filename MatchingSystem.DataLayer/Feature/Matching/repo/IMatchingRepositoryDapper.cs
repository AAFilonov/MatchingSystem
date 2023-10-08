namespace MatchingSystem.DataLayer.Feature.Matching.repo;

public interface IMatchingRepositoryDapper
{
    void SetNextStage(int matchingId);
}