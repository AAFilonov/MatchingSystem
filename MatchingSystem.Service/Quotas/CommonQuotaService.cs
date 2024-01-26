using System;
using System.Collections.Generic;
using System.Linq;
using Lombok.NET;
using MatchingSystem.DataLayer;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.CommonQuota.repo;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Stage.repo;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Quotas;

[AllArgsConstructor]
public partial class  CommonCommonQuotaService : ICommonQuotaService
{
    private readonly IProjectRepositoryOld projectRepositoryOld;
    private readonly IQuotaRequestRepository quotaRequestRepository;
    private readonly IMatchingRepository matchingRepository;
    private readonly IProjectRepository projectRepository;
    private readonly ITutorRepository tutorRepository;
    
    public TutorQuotaData Initialize(int tutorId, int stageTypeCode)
    {
        TutorEntity tutorEntity = tutorRepository.findById(tutorId);
        var model = new TutorQuotaData();
        model.History = quotaRequestRepository.GetQuotaRequestHistoryByTutor(tutorId, tutorEntity.MatchingId);
        model.CommonQuota = quotaRequestRepository.GetActiveTutorQuota(tutorId, tutorEntity.MatchingId)?.Qty;

        if (stageTypeCode == 4) model.Projects = projectRepositoryOld.GetProjectsByTutor(tutorId);
        return model;
    }

    public void CreateQuotaRequest(ChangeQuotaRequest data)
    {
        TutorEntity tutorEntity = tutorRepository.findById(data.TutorId);
        StageEntity stage = matchingRepository.GetCurrentStage(tutorEntity.MatchingId);

        quotaRequestRepository.CallInTransaction(context =>
        {
            var quotaRequest = new CommonQuotaEntity
            {
                TutorId = data.TutorId,
                Qty = data.NewQuotaQty,
                Message = data.Message,
                IsNotification = true,
                QuotaStateId = (int)CommonQuotaStateEnum.REQUESTED,
                UpdateDate = null,
                StageId = stage.StageId
            };
            quotaRequestRepository.save(quotaRequest, context);

            if (stage.StageTypeId == (int)StageTypeEnum.ITERATION && data.Deltas.Count > 0)
            {
                var updatedProjects = data.Deltas.Select(projectQuota =>
                {
                    ProjectEntity project = projectRepository.findById(projectQuota.ProjectID);
                    project.UpdateDate = DateTime.Now;
                    project.ProjectQuotaDelta = projectQuota.Quota;
                    return project;
                }).ToList();
                projectRepository.updateRange(updatedProjects, context);
            }
        });
    }
    
    public void AcceptRequest(int quotaId, string action)
    {
        quotaRequestRepository.CallInTransaction(context =>
        {
            CommonQuotaEntity commonQuotaRequest = quotaRequestRepository.findById(quotaId);
            if ("decline" == action)
            {
                DeclineRequest(commonQuotaRequest, context);
                return;
            }
            AcceptRequest(commonQuotaRequest, context);

           StageEntity stage =  matchingRepository.GetCurrentStage(commonQuotaRequest.Stage.MatchingId);
           if (stage.StageTypeId == (int)StageTypeEnum.ITERATION)
               UpdateTutorProjectQuotaDeltas(commonQuotaRequest, context);
        });
    }
    
    private void DeclineRequest(CommonQuotaEntity commonQuotaRequest, DiplomaMatchingContext context)
    {
        commonQuotaRequest.IsNotification = true;
        commonQuotaRequest.QuotaStateId = (int)CommonQuotaStateEnum.DECLINED;
        quotaRequestRepository.update(commonQuotaRequest, context);
    }
    
    private void AcceptRequest(CommonQuotaEntity commonQuotaRequest, DiplomaMatchingContext context)
    {
        var oldCommonQuota =
            quotaRequestRepository.GetActiveTutorQuota(commonQuotaRequest.TutorId, commonQuotaRequest.Stage.MatchingId);
        commonQuotaRequest.IsNotification = true;
        commonQuotaRequest.QuotaStateId = (int)CommonQuotaStateEnum.ACTIVE;
        oldCommonQuota.IsNotification = true;
        oldCommonQuota.QuotaStateId = (int)CommonQuotaStateEnum.UNACTIVE;
        quotaRequestRepository.updateRange(new[] { oldCommonQuota, commonQuotaRequest }, context);
    }
    
    private void UpdateTutorProjectQuotaDeltas(CommonQuotaEntity commonQuotaRequest, DiplomaMatchingContext context)
    {
        var projects = projectRepository.find(entity => entity.TutorId == commonQuotaRequest.TutorId);
        foreach (ProjectEntity project in projects)
        {
            project.ProjectQuotaDelta = null;
            project.UpdateDate = DateTime.Now;
        }
        projectRepository.updateRange(projects, context);
    }
}