
using MatchingSystem.Data.Model;

namespace MatchingSystem.Data.Feature.Matching;

public interface IMatchingRepository :  IRepository<Model.Matching>
{
    Stage GetCurrentStage(int matchingId);
}