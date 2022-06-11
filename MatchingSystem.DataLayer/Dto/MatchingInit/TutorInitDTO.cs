using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Dto.MatchingInit;

public class TutorInitDto
{
    public int id { get; set; }
    public string nameAbbreviation { get; set; }
    public bool isIncluded { get; set; }
    public int quota { get; set; }
    public List<GroupInitDto> groups { get; set; }
    
}