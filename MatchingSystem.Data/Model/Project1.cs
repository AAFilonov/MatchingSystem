namespace MatchingSystem.DataLayer.Model;

public class Project1
{
    public int TutorId { get; set; }
    public int UserId { get; set; }
    public string? TutorSurname { get; set; }
    public string? TutorName { get; set; }
    public string? TutorPatronimic { get; set; }
    public string? TutorNameAbbreviation { get; set; }
    public bool TutorIsClosed { get; set; }
    public int ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public string? Info { get; set; }
    public bool? IsClosed { get; set; }
    public bool? IsDefault { get; set; }
    public int? MatchingId { get; set; }
    public short? Qty { get; set; }
    public string? QtyDescription { get; set; }
    public string? AvailableGroupsNameList { get; set; }
    public string? TechnologiesNameList { get; set; }
    public string? WorkDirectionsNameList { get; set; }
}