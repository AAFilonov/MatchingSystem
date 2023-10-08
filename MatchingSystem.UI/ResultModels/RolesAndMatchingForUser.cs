using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ResultModels
{
    public class RolesAndMatchingForUser
    {
        public List<MatchingEntity> Matchings { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}