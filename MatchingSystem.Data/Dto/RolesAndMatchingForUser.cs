using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Dto;
public class RolesAndMatchingForUser
{
    public List<Matching> Matchings { get; set; }
    public List<Role> Roles { get; set; }
}