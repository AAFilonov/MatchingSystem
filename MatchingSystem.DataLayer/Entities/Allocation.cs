namespace MatchingSystem.DataLayer.Entities
{
    #nullable enable
    public class Allocation
    {
        public int? MatchingID { get; set; }
        public int? ProjectID { get; set; }
        public int? StudentID { get; set; }
        public int? TutorID { get; set; }
        public int? GroupID { get; set; }
        public string? StudentNameAbbreviation { get; set; }
        public string? GroupName { get; set; }
        public string? ProjectName { get; set; }
        public string? TutorNameAbbreviation { get; set; }
        public bool? IsAllocated { get; set; }
    }
}