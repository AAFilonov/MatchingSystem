using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.CommonQuota.repo;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.OldDto;

namespace MatchingSystem.Service.Quotas;

public class CommonCommonQuotaService : ICommonQuotaService
{

    private readonly IProjectRepositoryOld projectRepositoryOld;
    private readonly ICommonQuotaRepository commonQuotaRepository;
    private readonly IMatchingRepository matchingRepository;
    private readonly ITutorRepository tutorRepository;

    public CommonCommonQuotaService(
        IProjectRepositoryOld projectRepositoryOld,ICommonQuotaRepository commonQuotaRepository, IMatchingRepository matchingRepository, ITutorRepository tutorRepository)
    {
        this.projectRepositoryOld = projectRepositoryOld;
        this.commonQuotaRepository = commonQuotaRepository;
        this.matchingRepository = matchingRepository;
        this.tutorRepository = tutorRepository;
    }

    public TutorQuotaData Initialize(int tutorId, int stageTypeCode)
    {
        var tutorEntity = tutorRepository.findById(tutorId);
        var model = new TutorQuotaData();
        model.History = commonQuotaRepository.GetQuotaRequestHistoryByTutor(tutorId, tutorEntity.MatchingId);
        model.CommonQuota = commonQuotaRepository.GetActiveTutorQuota(tutorId, tutorEntity.MatchingId)?.Qty;

        if (stageTypeCode == 4) model.Projects = projectRepositoryOld.GetProjectsByTutor(tutorId);
        return model;
    }

    public void CreateQuotaRequest(ChangeQuotaRequest data)
    {
        TutorEntity tutorEntity = tutorRepository.findById(data.TutorId);
        StageEntity stage = matchingRepository.GetCurrentStage(tutorEntity.MatchingId);
        
        var quotaEntity = new CommonQuotaEntity
        {
            TutorId = data.TutorId,
            Qty = data.NewQuotaQty,
            Message = data.Message,
            IsNotification = true,
            QuotaStateId = (int)CommonQuotaStateEnum.REQUESTED,
            UpdateDate = null,
            StageId = stage.StageId
        };
        commonQuotaRepository.save(quotaEntity);
    }

    public void CreateRequestForLastStage(ChangeQuotaRequest data)
    {
        //TODO  DPL-33
        CreateQuotaRequest(data);
    }
    
    public void AcceptRequest(int quotaId, string action)
    {
        CommonQuotaEntity commonQuotaRequest =  commonQuotaRepository.findById(quotaId);
        if ("decline" == action)
        {
            commonQuotaRequest.IsNotification = true;
            commonQuotaRequest.QuotaStateId = (int)CommonQuotaStateEnum.DECLINED;
            commonQuotaRepository.update(commonQuotaRequest);
            return;
        }

        var oldCommonQuota = commonQuotaRepository.GetActiveTutorQuota(commonQuotaRequest.TutorId, commonQuotaRequest.Stage.MatchingId);
        commonQuotaRequest.IsNotification = true;
        commonQuotaRequest.QuotaStateId = (int)CommonQuotaStateEnum.ACTIVE;
        oldCommonQuota.IsNotification = true;
        oldCommonQuota.QuotaStateId = (int)CommonQuotaStateEnum.UNACTIVE;
        commonQuotaRepository.updateRange(new[] { oldCommonQuota, commonQuotaRequest });
        //TODO DPL-33 если stage  это итерация то нужно что то сделать со студентами.
    }
}