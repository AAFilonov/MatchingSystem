using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Stage.service;

public class StageRepository: Repository<StageEntity>, IStageRepository
{
    public StageRepository(DbContext context) : base(context)
    {
    }
}