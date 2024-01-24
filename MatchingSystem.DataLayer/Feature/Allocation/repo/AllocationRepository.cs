using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.TutorChoise;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.DataLayer.OldDto.IO.Params;

namespace MatchingSystem.DataLayer.Feature.Allocation.repo;

public class AllocationRepository :  Repository, IAllocationRepository
{
    private const int EXECUTIVE_ROLE_ID = 3;
    private const int CHOSING_TYPE_BY_EXECUTIVE_ID = 4;
    private readonly  IMatchingRepository matchingRepository;
    
    public AllocationRepository(IMatchingRepository matchingRepository) 
    {
        this.matchingRepository = matchingRepository;
    }

    public IEnumerable<AllocationDto> GetAllocationsByExecutive(int userId, int matchingId)
    {
        using (var context = _matchingSystemContext)
        {
            return context.AllocationFullInfos
                .Join(context.UsersRoles,
                    view => view.MatchingId,
                    ur => ur.MatchingId,
                    (view, ur) => new AllocationDto()
                    {
                        MatchingID = view.MatchingId,
                        ProjectID = view.ProjectId,
                        StudentID = view.StudentId,
                        TutorID = view.TutorId,
                        StudentNameAbbreviation = view.StudentNameAbbreviation,
                        GroupName = view.GroupName,
                        ProjectName = view.ProjectName,
                        TutorNameAbbreviation = view.TutorNameAbbreviation,
                        IsAllocated = view.IsAllocated == 1,
                        UserID = ur.UserId,
                        UserRoleID = ur.UserRoleId
                    }).Where(dto =>
                    dto.MatchingID == matchingId &&
                    dto.UserID == userId &&
                    dto.UserRoleID == EXECUTIVE_ROLE_ID);
        }

    }

    public void SetAdjustmentByExecutive(AdjustmentRequest request)
    {
        foreach (var student in request.Allocations)
        {
            var inParams = new AdjustmentParams
            {
                MatchingId = request.MatchingID.Value,
                ProjectId = student.ProjectID.Value,
                StudentId = student.StudentID.Value,
                UserId = request.UserID.Value
            };
           
        }

      StageEntity currentStage = matchingRepository.GetCurrentStage((int)request.MatchingID);
      using (var context = _matchingSystemContext)
      {
          context.TutorsChoices.AddRange(
              request.Allocations.Select(student =>new TutorsChoiceEntity()
              {
                  StudentId = student.StudentID.Value,
                  ProjectId = student.ProjectID.Value,
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