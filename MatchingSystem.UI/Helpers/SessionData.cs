using System;
using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.UI.Helpers
{
    public class SessionData
    {
        public User? User { get;set; }
        public List<RoleMatching>? RolesMatchings { get; set; }
        public int? SelectedMatching { get; set; }
        public string? SelectedRole { get; set; }
        public int? CountRoles { get; set; }
        public Stage? CurrentStage { get; set; }
        public int? TutorID { get; set; }
    }
}
