using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Dto;
public class RolesAndMatchingForUser
{
    public List<MatchingDto> Matchings { get; set; }
    public List<RoleDto> Roles { get; set; }
}