using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Dto;

public class TutorDto
{
    public int id { get; set; }
    public string nameAbbreviation { get; set; }
    public bool isIncluded { get; set; }
    public int quota { get; set; }
    public List<GroupTutorDto> groups { get; set; }
    
}