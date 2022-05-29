﻿using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.Data.Model;
using MatchingSystem.DataLayer.Dto;
using Stage = MatchingSystem.DataLayer.Entities.Stage;
using Tutor = MatchingSystem.DataLayer.Entities.Tutor;
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

        public MatchingInitData matchingInitData { get; set; } = new MatchingInitData();
    }

    public class MatchingInitData
    {
        public StudentDto[] studentDtos { get; set; }
        public List<TutorDto> tutorDtos { get; set; }
        public  List<GroupTutorDto>  groupTutorDtos { get; set; }
    }
}
