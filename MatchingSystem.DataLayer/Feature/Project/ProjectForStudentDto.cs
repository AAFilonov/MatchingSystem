using System.ComponentModel.DataAnnotations;

namespace MatchingSystem.DataLayer.Feature.Project
{
    #nullable enable
    public class ProjectForStudentDto
    {
        [Key]
        public int ProjectID { get; set; }
        public string? TutorNameAbbreviation { get; set; }
        public string? ProjectName { get; set; }
        public string? Info { get; set; }
        public string? TechnologiesName_List { get; set; }
        public string? WorkDirectionsName_List { get; set; }
        public bool? IsSelectedByStudent { get; set; }
        public short? OrderNumber { get; set; }
    }
}
