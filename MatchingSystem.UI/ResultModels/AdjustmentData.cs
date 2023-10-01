using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ResultModels
{
    public class AdjustmentData
    {
        public List<TutorDto> Tutors { get; set; }
        public List<Allocation> Allocations { get; set; }
        public List<Project> Projects { get; set; }
    }
}