using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.MatchingInit;

namespace MatchingSystem.Service.MatchingInitialization;

public interface IMatchingInitializationService
{
    public void createMatching(MatchingInitData data,int creatorUserId);
}