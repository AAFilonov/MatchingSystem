using MatchingSystem.DataLayer.Dto;

namespace MatchingSystem.Service.Quotas;

public interface IQuotasService
{
    public TutorQuotaData Initialize(int tutorId, int stageTypeCode);

    public void CreateRequest(ChangeQuotaRequest data);

    public void CreateRequestForLastStage(ChangeQuotaRequest data);

    public void AcceptRequest(int quotaid, string action);

}
