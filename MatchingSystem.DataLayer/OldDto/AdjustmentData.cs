using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Model;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Dto;
public class AdjustmentData
{
    public IEnumerable<TutorView> Tutors { get; set; }
    public IEnumerable<AllocationDto> Allocations { get; set; }
    public List<ProjectDto> Projects { get; set; }
}