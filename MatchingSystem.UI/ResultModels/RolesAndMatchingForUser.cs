using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.UI.ResultModels
{
    public class RolesAndMatchingForUser
    {
        public List<Matching> Matchings { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}