using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldDto.IO.Params;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.CommonQuota;

public class CommonQuotaRepository : GenericRepository<CommonQuotaEntity>, ICommonQuotaRepository
{
    public void CreateCommonQuotaRequestForLastStage(CreateCommonQuotaParams @params)
    {
        throw new NotImplementedException();
    }

    public void CreateCommonQuotaRequestForSecondStage(int tutorId, int quota, string message)
    {
        throw new NotImplementedException();
    }

    public void CreateCommonQuotaRequestForThirdStage(int tutorId, int quota, string message)
    {
        throw new NotImplementedException();
    }

    public void CreateCommonQuotas(List<TutorInitDto> tuts, int stageId,
        DiplomaMatchingContext? extContext = null)
    {
        var quotas = tuts.Select(info =>
        {
            var commonQuota = new CommonQuotaEntity
            {
                TutorId = info.TutorId,
                Qty = (short)info.quota,
                StageId = stageId,
                CreateDate = DateTime.Now,
                QuotaStateId = (int)CommonQuotaStateEnum.ACTIVE
            };

            return commonQuota;
        });
        saveRange(quotas, extContext);
    }


    public IEnumerable<QuotaHistoryTutor> GetQuotaRequestHistoryByTutor(int tutorId)
    {
        throw new NotImplementedException();
    }
}