using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Matching.repo;

public class MatchingRepository : GenericRepository<MatchingEntity>, IMatchingRepository
{
    public IEnumerable<AllocationDto> GetFinalAllocations()
    {
        List<AllocationFullInfoView> allocations = _matchingSystemContext.AllocationFullInfos.ToList();
        return allocations.Select(info =>
        {
            AllocationDto dto = new AllocationDto();
            dto.GroupName = info.GroupName;
            dto.IsAllocated = info.IsAllocated==1;
            dto.ProjectName = info.ProjectName;
            dto.GroupId = info.GroupId;
            dto.ProjectId = info.ProjectId;
            dto.MatchingId = info.MatchingId;
            dto.StudentId = info.StudentId;
            dto.StudentNameAbbreviation = info.StudentNameAbbreviation;
            dto.TutorId = info.TutorId;
            dto.TutorNameAbbreviation = info.TutorNameAbbreviation;
            return dto;
        });
        
    }

    public IEnumerable<AllocationDto> GetFinalAllocationByMatching(int MatchingId)
    {
        return GetFinalAllocations().Where(allocation => allocation.MatchingId == MatchingId);
    }

    public StageDto GetCurrentStageDto(int matchingId)
    {
        var stage = GetCurrentStage(matchingId);
        return StageDto.fromEntity(stage);
    }

    public StageEntity GetCurrentStage(int matchingId)
    {
        return findById(matchingId).Stages.Single(entity => entity.IsCurrent == true);
    }

    public IEnumerable<MatchingDto> GetMatchingsDto()
    {
        return _matchingSystemContext.Matchings.Select(matching => new MatchingDto(matching));
    }
}