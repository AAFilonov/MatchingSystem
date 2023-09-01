
using MatchingSystem.DataLayer.Model;

namespace MatchingSystem.DataLayer.Feature.Matching;

public interface IMatchingRepository :  IRepository<Data.Model.Matching>
{
    Stage GetCurrentStage(int matchingId);
}