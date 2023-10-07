using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.IO.Params;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.CommonQuota;

public interface ICommonQuotaRepository : IRepository<CommonQuotaEntity>
{
    public void CreateCommonQuotaRequestForLastStage(CreateCommonQuotaParams @params);
    public void CreateCommonQuotaRequestForSecondStage(int tutorId, int quota, string message);
    public void CreateCommonQuotaRequestForThirdStage(int tutorId, int quota, string message);
    public void SetCommonQuotasForTutors(List<TutorInitDto> tuts, int stageId);
    IEnumerable<QuotaHistoryTutor> GetQuotaRequestHistoryByTutor(int tutorId);
}