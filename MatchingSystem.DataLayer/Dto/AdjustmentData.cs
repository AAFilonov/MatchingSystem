using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Dto;
public class AdjustmentData
{
    public IEnumerable<TutorDto> Tutors { get; set; }
    public IEnumerable<Allocation> Allocations { get; set; }
    public List<Project> Projects { get; set; }
}