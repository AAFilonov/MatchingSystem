using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Dto;
using Stage = MatchingSystem.DataLayer.Entities.Stage;
using User = MatchingSystem.Data.Model.User;

namespace MatchingSystem.UI.Helpers
{
    public class SessionData
    {
        public User User { get; set; }
        public IEnumerable<RoleMatching> RolesMatchings { get; set; }
        public int SelectedMatching { get; set; }

        public string MatchingTypeCode { get; set; }
        public string SelectedRole { get; set; }
        public int CountRoles { get; set; }
        public Stage CurrentStage { get; set; }
        public int TutorId { get; set; }

        public MatchingInitData matchingInitData { get; set; } = new MatchingInitData();
    }
    
}