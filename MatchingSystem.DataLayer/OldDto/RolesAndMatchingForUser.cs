using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Role;

namespace MatchingSystem.DataLayer.OldDto;
public class RolesAndMatchingForUser
{
    public List<MatchingDto> Matchings { get; set; }
    public List<RoleDto> Roles { get; set; }
}