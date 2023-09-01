using System.ComponentModel.DataAnnotations.Schema;

namespace MatchingSystem.DataLayer.OldEntities
{
    #nullable enable
    public class Project
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
        [NotMapped]
        public int? TutorID { get; set; }
    }
}
