using System.Linq;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.OldDto;

namespace MatchingSystem.Service.Allocation;

public class AllocationService : IAllocationService
{
    private readonly IMatchingRepository matchingRepository;

    public AllocationService(IMatchingRepository matchingRepository)
    {
        this.matchingRepository = matchingRepository;
    }

    public AllocationData GetAllocations()
    {
        var allocations = matchingRepository.GetAllocationsFullInfoViews();
        var allocationsDtos= allocations.Select(info =>
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
        
        var matchings = matchingRepository.findAll();
        var matchingDtos = matchings.Select(entity => new MatchingDto(entity));
        
        var model = new AllocationData
        {
            Allocations = allocationsDtos,
            Matchings = matchingDtos
        };
        return model;
    }
    
}