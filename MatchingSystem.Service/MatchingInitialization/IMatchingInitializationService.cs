using MatchingSystem.DataLayer.Dto;

namespace MatchingSystem.Service.MatchingInitialization;

public interface IMatchingInitializationService
{
    public void createMatching(MatchingInitData data);
}