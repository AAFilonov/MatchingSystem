using System;

namespace MatchingSystem.DataLayer.Dto.MatchingMonitoring;

public class TutorFullDTO
{
    public int? TutorID;
    public int? UserID;
    public int? MatchingID;
    public bool? IsClosed;
    public DateTime? LastVisitDate;
    public string NameAbbreviation;
    public short? Quota;
}