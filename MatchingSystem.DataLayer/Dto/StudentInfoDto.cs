using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Dto;

public class StudentInfoDto
{
    public IEnumerable<WorkDirectionDto> WorkDirections { get; set; }
    public IEnumerable<TechnologyDto> Technologies { get; set; }
    public string Info { get; set; }
    public string Info2 { get; set; }
}
