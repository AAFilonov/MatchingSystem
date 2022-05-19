namespace MatchingSystem.Data.Feature.Project;
#nullable enable
public class ProjectDto
{
        public int? ProjectID { get; set; }
        public string? ProjectName { get; set; }
        public string? Info { get; set; }
        public bool? IsClosed { get; set; }
        public short? Qty { get; set; }
        public string? AvailableGroupsName_List { get; set; }
        public string? TechnologiesName_List { get; set; }
        public string? WorkDirectionsName_List { get; set; }
        public bool? IsDefault { get; set; }
        public int? TutorID { get; set; }
}