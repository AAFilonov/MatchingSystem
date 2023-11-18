using System.Collections.Generic;
using MatchingSystem.DataLayer.OldDto.MatchingInit;

namespace MatchingSystem.DataLayer.Feature.Tutor;

public class TutorInitDto
{
    public int UserId { get; set; }
    public int TutorId { get; set; }
    public int DefaultProjectId { get; set; } 
    public string nameAbbreviation { get; set; }
    public bool isIncluded { get; set; }
    public int quota { get; set; }
    public List<GroupInitDto> groups { get; set; }
    
}