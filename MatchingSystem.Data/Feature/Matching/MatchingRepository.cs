using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Data.Feature.Matching;

public class MatchingRepository :  Repository<Model.Matching>,IMatchingRepository
{
    public MatchingRepository(DbContext context) : base(context)
    {
    }
      
    public Stage GetCurrentStage(int matchingId)
    {
        throw new NotImplementedException();
    }

}