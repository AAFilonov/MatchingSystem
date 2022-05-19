using MatchingSystem.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Data.Feature.Stage;

public class StageRepository : Repository<Model.Stage>, IStageRepository
{
    public StageRepository(DbContext context) : base(context)
    {
    }
}