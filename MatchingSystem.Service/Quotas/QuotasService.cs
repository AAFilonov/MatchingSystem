
using System.Linq;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.DataLayer.OldDto.IO.Params;
using MatchingSystem.Service.Executive;

namespace MatchingSystem.Service.Quotas;

public class QuotasService : IQuotasService
{
    private readonly IExecutiveServiceOld executiveService;
    private readonly IProjectRepositoryOld projectRepositoryOld;
    private readonly ICommonQuotaRepository commonQuotaRepository;

    public QuotasService(
        IProjectRepositoryOld projectRepositoryOld,
        IExecutiveServiceOld executiveService, ICommonQuotaRepository commonQuotaRepository)
    {
        this.projectRepositoryOld = projectRepositoryOld;
        this.executiveService = executiveService;
        this.commonQuotaRepository = commonQuotaRepository;
    }

    public TutorQuotaData Initialize(int tutorId, int stageTypeCode)
    {
        var model = new TutorQuotaData
        {
            History = commonQuotaRepository.GetQuotaRequestHistoryByTutor(tutorId),
            CommonQuota = commonQuotaRepository.find( entity =>entity.TutorId== tutorId).First().Qty
        };
        if (stageTypeCode == 4) model.Projects = projectRepositoryOld.GetProjectsByTutor(tutorId);

        return model;
    }

    public void CreateRequest(ChangeQuotaRequest data)
    {
        commonQuotaRepository.CreateCommonQuotaRequestForSecondStage(data.TutorId, data.NewQuotaQty, data.Message);
    }

    public void CreateRequestForLastStage(ChangeQuotaRequest data)
    {
        var request = new CreateCommonQuotaParams
        {
            Message = data.Message,
            NewQuota = data.NewQuotaQty,
            TutorId = data.TutorId
        };
        request.FillProjectQuota(data.Deltas);

        commonQuotaRepository.CreateCommonQuotaRequestForLastStage(request);
    }
    
    public void AcceptRequest(int quotaid, string action)
    {
        executiveService.AcceptQuotaRequest(quotaid, action == "accept");
    }
}