using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.DataLayer.OldDto;
public class AdjustmentData
{
    public IEnumerable<TutorView> Tutors { get; set; }
    public IEnumerable<AllocationDto> Allocations { get; set; }
    public List<ProjectDto> Projects { get; set; }
}