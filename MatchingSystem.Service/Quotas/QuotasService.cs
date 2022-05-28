using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.IO.Params;

namespace Service.Quotas;
public class QuotasService: IQuotasService
{
    private readonly ITutorRepository tutorRepository;
    private readonly IProjectRepository projectRepository;
    private readonly IExecutiveRepository executiveRepository;

    public QuotasService(
        ITutorRepository tutorRepository,
        IProjectRepository projectRepository,
        IExecutiveRepository executiveRepository
    )
    {
        this.tutorRepository = tutorRepository;
        this.projectRepository = projectRepository;
        this.executiveRepository = executiveRepository;
    }

    public TutorQuotaData Initialize(int tutorId, int stageTypeCode)
    {
        var model = new TutorQuotaData
        {
            History = tutorRepository.GetQuotaRequestHistoryByTutor(tutorId),
            CommonQuota = tutorRepository.GetCommonQuotaByTutor(tutorId),
        };
        if (stageTypeCode == 4)
        {
            model.Projects = projectRepository.GetProjectsByTutor(tutorId);
        }

        return model;
    }

    public void CreateRequest(ChangeQuotaRequest data)
    {
        tutorRepository.CreateCommonQuotaRequestForSecondStage(data.TutorId, data.NewQuotaQty, data.Message);
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

        tutorRepository.CreateCommonQuotaRequestForLastStage(request);
    }


    public void AcceptRequest(int quotaid,string action)
    {
        executiveRepository.AcceptQuotaRequest(quotaid, action == "accept");
    }
}
