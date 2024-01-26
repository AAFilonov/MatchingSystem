using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.DataLayer.Feature.CommonQuota.repo;

public interface ICommonQuotaRepository : IRepository<CommonQuotaEntity>
{
    public void CreateCommonQuotas(List<TutorInitDto> tuts, int stageId, DiplomaMatchingContext? extContext = null);
}