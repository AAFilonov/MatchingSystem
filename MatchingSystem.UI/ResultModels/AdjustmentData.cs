using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Allocation;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.UI.ResultModels
{
    public class AdjustmentData
    {
        public List<TutorDto> Tutors { get; set; }
        public List<AllocationDto> Allocations { get; set; }
        public List<ProjectDto> Projects { get; set; }
    }
}