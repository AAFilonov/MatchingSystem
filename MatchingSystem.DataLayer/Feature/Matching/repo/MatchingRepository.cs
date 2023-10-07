using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Model;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Matching.repo;

public class MatchingRepository : GenericRepository<MatchingEntity>, IMatchingRepository
{

    public MatchingRepository(DbContext context) : base(context)
    {
    }


    public IEnumerable<AllocationDto> GetFinalAllocations()
    {
        List<AllocationFullInfoView> allocations = _matchingSystemContext.AllocationFullInfos.ToList();
        return allocations.Select(info =>
        {
            AllocationDto dto = new AllocationDto();
            dto.GroupName = info.GroupName;
            dto.IsAllocated = info.IsAllocated==1;
            dto.ProjectName = info.ProjectName;
            dto.GroupID = info.GroupId;
            dto.ProjectID = info.ProjectId;
            dto.MatchingID = info.MatchingId;
            dto.StudentID = info.StudentId;
            dto.StudentNameAbbreviation = info.StudentNameAbbreviation;
            dto.TutorID = info.TutorId;
            dto.TutorNameAbbreviation = info.TutorNameAbbreviation;
            return dto;
        });
        
    }

    public IEnumerable<AllocationDto> GetFinalAllocationByMatching(int MatchingId)
    {
        return GetFinalAllocations().Where(allocation => allocation.MatchingID == MatchingId);
    }

    public StageDto GetCurrentStageDto(int matchingId)
    {
        var stage = GetCurrentStage(matchingId);
        return StageDto.fromEntity(stage);
    }

    public Stage.StageEntity GetCurrentStage(int matchingId)
    {
        List<Stage.StageEntity> stages = _matchingSystemContext.Matchings.Find(matchingId)?.Stages.ToList();
        Stage.StageEntity currentStage = stages.Where(stage => stage.IsCurrent == true).FirstOrDefault();
        return currentStage;
    }

    public IEnumerable<MatchingDto> GetMatchingsDto()
    {
        return _matchingSystemContext.Matchings.Select(matching => new MatchingDto(matching));
    }
    
    public IEnumerable<MatchingEntity> GetMatchings()
    {
        return _matchingSystemContext.Matchings.Include(matching =>matching.MatchingType );
    }
}