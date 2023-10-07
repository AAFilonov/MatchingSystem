using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Stage.repo;

public class StageRepository: GenericRepository<StageEntity>, IStageRepository
{
    public StageRepository(DbContext context) : base(context)
    {
    }
}