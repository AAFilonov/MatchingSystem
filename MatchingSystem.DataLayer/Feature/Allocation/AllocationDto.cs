namespace MatchingSystem.DataLayer.Feature.Allocation
{
    #nullable enable
    public class AllocationDto
    {
        public int? MatchingId { get; set; }
        public int? ProjectId { get; set; }
        public int? StudentId { get; set; }
        public int? TutorId { get; set; }
        public int? GroupId { get; set; }
        public string? StudentNameAbbreviation { get; set; }
        public string? GroupName { get; set; }
        public string? ProjectName { get; set; }
        public string? TutorNameAbbreviation { get; set; }
        public bool? IsAllocated { get; set; }
        public int? UserId { get; set; }
        public int? UserRoleId { get; set; }
    }
}