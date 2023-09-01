using System.Collections.Generic;
using MatchingSystem.Data.Model;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.OldEntities;
using Stage = MatchingSystem.DataLayer.OldEntities.Stage;
using Tutor = MatchingSystem.DataLayer.OldEntities.Tutor;
using User = MatchingSystem.Data.Model.User;

namespace MatchingSystem.UI.Helpers
{
    public class SessionData
    {
        public User User { get;set; }
        public IEnumerable<RoleMatching> RolesMatchings { get; set; }
        public int SelectedMatching { get; set; }
        
        public string MatchingTypeCode { get; set; }
        public string SelectedRole { get; set; }
        public int CountRoles { get; set; }
        public Stage CurrentStage { get; set; }
        public int TutorId { get; set; }

        public MatchingInitData matchingInitData { get; set; } = new();
    }
}
