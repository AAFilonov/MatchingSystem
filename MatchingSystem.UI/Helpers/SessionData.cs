using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.User;
using MatchingSystem.DataLayer.OldEntities;


namespace MatchingSystem.UI.Helpers
{
    public class SessionData
    {
        public UserDto UserDto { get;set; }
        public IEnumerable<RoleMatching> RolesMatchings { get; set; }
        public int SelectedMatching { get; set; }
        
        public string MatchingTypeCode { get; set; }
        public string SelectedRole { get; set; }
        public int CountRoles { get; set; }
        public StageDto CurrentStage { get; set; }
        public int TutorId { get; set; }

        public MatchingInitData matchingInitData { get; set; } = new();
    }
}
