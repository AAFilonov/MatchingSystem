#nullable enable
using System;

namespace MatchingSystem.DataLayer.Entities
{
    public class StatisticsStudentsProjects
    {
        public int? StudentsPreferencesProjectID { get; set; }
        public string? ProjectsProjectName { get; set; }
        public string? ProjectsTechnologiesName_List { get; set; }
        public string? ProjectsWorkDirectionsName_List { get; set; }
        public int? ProjectQty { get; set; }
        public string? ProjectsAvailableGroupsName_List { get; set; }
        public int? TutorID { get; set; }
        public string? TutorNameAbbreviation  { get; set; }
        public int? OrderNumber { get; set; }
     

    }
}