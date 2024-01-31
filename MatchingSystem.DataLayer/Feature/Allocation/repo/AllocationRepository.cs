using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.TutorChoice;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.DataLayer.OldDto.IO.Params;

namespace MatchingSystem.DataLayer.Feature.Allocation.repo;

public class AllocationRepository : Repository, IAllocationRepository
{
    private const int CHOSING_TYPE_BY_EXECUTIVE_ID = 4;
    private readonly IMatchingRepository matchingRepository;

    public AllocationRepository(IMatchingRepository matchingRepository)
    {
        this.matchingRepository = matchingRepository;
    }

    public IEnumerable<AllocationDto> GetAllocationsByExecutive(int userId, int matchingId)
    {
        return CallInContext<IEnumerable<AllocationDto>>(context =>
        {
            var views = context.AllocationFullInfos.Where(view => view.MatchingId == matchingId).ToList();
            return views.Select(view =>
            {
                var dto = new AllocationDto();
                dto.MatchingId = matchingId;
                dto.ProjectId = view.ProjectId;
                dto.StudentId = view.StudentId;
                dto.TutorId = view.TutorId;
                dto.StudentNameAbbreviation = view.StudentNameAbbreviation;
                dto.GroupName = view.GroupName;
                dto.ProjectId = view.ProjectId;
                dto.ProjectName = view.ProjectName;
                dto.TutorNameAbbreviation = view.TutorNameAbbreviation;
                dto.IsAllocated = view.IsAllocated == 1;
                dto.UserId = userId;
                dto.UserRoleId = (int)RolesEnum.EXECUTIVE;
                return dto;
            });
        });
    }

    public void SetAdjustmentByExecutive(AdjustmentRequest request)
    {
        foreach (AllocationDto student in request.Allocations)
        {
            var inParams = new AdjustmentParams
            {
                MatchingId = request.MatchingID.Value,
                ProjectId = student.ProjectId.Value,
                StudentId = student.StudentId.Value,
                UserId = request.UserID.Value
            };
        }

        StageEntity currentStage = matchingRepository.GetCurrentStage((int)request.MatchingID);
        using (DiplomaMatchingContext context = _matchingSystemContext)
        {
            context.TutorsChoices.AddRange(
                request.Allocations.Select(student => new TutorsChoiceEntity
                {
                    StudentId = student.StudentId.Value,
                    ProjectId = student.ProjectId.Value,
                    IsInQuota = true,
                    IsChangeble = false,
                    TypeId = CHOSING_TYPE_BY_EXECUTIVE_ID,
                    StageId = currentStage.StageId,
                    CreateDate = DateTime.Now,
                    IsFromPreviousIteration = false
                })
            );
            context.SaveChanges();
        }
    }
}