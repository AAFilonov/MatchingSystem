using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Allocation.repo;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.DataLayer.OldEntities;
using MatchingSystem.Service.Matching;

namespace MatchingSystem.Service.Executive;

public class ExecutiveService : IExecutiveService
{
    private readonly IMatchingService matchingService;
    private readonly IProjectRepositoryOld projectRepositoryOld;
    private readonly ITutorRepository tutorRepository;
    private readonly IAllocationRepository allocationRepository;

    public ExecutiveService(
        ITutorRepository tutorRepository,
        IProjectRepositoryOld projectRepositoryOld, IMatchingService matchingService, IAllocationRepository allocationRepository)
    {
        this.tutorRepository = tutorRepository;
        this.projectRepositoryOld = projectRepositoryOld;
        this.matchingService = matchingService;
        this.allocationRepository = allocationRepository;
    }

    public void SetNextStage(int? matchingId, int? userId)
    {
        matchingService.SetNextStage(matchingId.Value);
    }

    public AdjustmentData GetAllocationByExecutive(int? userId, int? matchingId)
    {
        var model = new AdjustmentData
        {
            Allocations = allocationRepository.GetAllocationsByExecutive(userId.Value, matchingId.Value)
                .Where(allocation => allocation.StudentID != null).ToList(),
            Tutors = tutorRepository.GetTutorsViews().Where(view => view.MatchingId == matchingId.Value),
            Projects = new List<ProjectDto>()
        };

        var tutorIds = model.Tutors.Select(x => x.TutorId).ToList();

        foreach (var tutorId in tutorIds)
        {
            var temp = projectRepositoryOld.GetProjectsByTutor(tutorId).ToList();
            temp.ForEach(x => x.TutorID = tutorId);
            model.Projects.AddRange(temp);
        }

        return model;
    }
    
    public void AcceptQuotaRequest(int quotaId, bool result)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<QuotaRequest> GetQuotaRequestsByExecutive(int userId, int matchingId)
    {
        throw new NotImplementedException();
    }

    public void SetEndDate(DateTime endDate, int matchingId)
    {
        matchingService.SetStageEndDate(endDate, matchingId);
    }
}