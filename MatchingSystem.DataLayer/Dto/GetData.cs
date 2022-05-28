using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Dto;

public class GetData
{
    public IEnumerable<WorkDirection> WorkDirections { get; set; }
    public IEnumerable<Technology> Technologies { get; set; }
    public string Info { get; set; }
    public string Info2 { get; set; }
}
