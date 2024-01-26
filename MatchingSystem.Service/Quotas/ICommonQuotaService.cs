using MatchingSystem.DataLayer.OldDto;

namespace MatchingSystem.Service.Quotas;

public interface ICommonQuotaService
{
    public TutorQuotaData Initialize(int tutorId, int stageTypeCode);
    public void CreateQuotaRequest(ChangeQuotaRequest data);
    public void AcceptRequest(int quotaId, string action);
}