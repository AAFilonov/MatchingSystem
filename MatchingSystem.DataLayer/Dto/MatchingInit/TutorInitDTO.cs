using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Dto.MatchingInit;

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