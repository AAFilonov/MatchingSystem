using MatchingSystem.DataLayer.OldDto.MatchingInit;

namespace MatchingSystem.Service.MatchingInitialization;

public interface IMatchingInitializationService
{
    public MatchingInitData InitNewMatching(MatchingInitData data);
}