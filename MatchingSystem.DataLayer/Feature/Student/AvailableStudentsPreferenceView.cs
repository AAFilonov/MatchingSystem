namespace MatchingSystem.DataLayer.Feature.Student;

public class AvailableStudentsPreferenceView
{
    public int PreferenceId { get; set; }
    public int StudentId { get; set; }
    public int ProjectId { get; set; }
    public short OrderNumber { get; set; }
    public bool? IsInUse { get; set; }
    public int? MatchingId { get; set; }
}