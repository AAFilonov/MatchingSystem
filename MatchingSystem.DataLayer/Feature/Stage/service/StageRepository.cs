using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Stage.service;

public class StageRepository: Repository<Stage>, IStageRepository
{
    public StageRepository(DbContext context) : base(context)
    {
    }
}