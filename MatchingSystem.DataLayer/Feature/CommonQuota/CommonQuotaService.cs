using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Dto.IO.Params;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.CommonQuota;

public class CommonQuotaRepository :GenericRepository<CommonQuotaEntity>, ICommonQuotaRepository
{
    public CommonQuotaRepository(DbContext context) : base(context)
    {
    }

    public void CreateCommonQuotaRequestForLastStage(CreateCommonQuotaParams @params)
    {
        throw new System.NotImplementedException();
    }

    public void CreateCommonQuotaRequestForSecondStage(int tutorId, int quota, string message)
    {
        throw new System.NotImplementedException();
    }

    public void CreateCommonQuotaRequestForThirdStage(int tutorId, int quota, string message)
    {
        throw new System.NotImplementedException();
    }
    
    public void SetCommonQuotasForTutors(List<TutorInitDto> tuts, int stageId)
    {
        var quotas = tuts.Select(info =>
        {
            var commonQuota = new CommonQuotaEntity()
            {
                TutorId = info.TutorId,
                Qty = (short)info.quota,
                StageId = stageId,
            };

            return commonQuota;
        });

        _matchingSystemContext.CommonQuotas.AddRange(quotas);
    }

    public IEnumerable<QuotaHistoryTutor> GetQuotaRequestHistoryByTutor(int tutorId)
    {
        throw new System.NotImplementedException();
    }
}