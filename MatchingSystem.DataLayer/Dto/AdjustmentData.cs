using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Dto;
public class AdjustmentData
{
    public IEnumerable<Tutor> Tutors { get; set; }
    public IEnumerable<Allocation> Allocations { get; set; }
    public List<Project> Projects { get; set; }
}