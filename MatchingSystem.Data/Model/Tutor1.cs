using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class Tutor1
{
    public int TutorId { get; set; }
    public bool IsClosed { get; set; }
    public bool? IsReadyToStart { get; set; }
    public int UserId { get; set; }
    public int? MatchingId { get; set; }
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Patronimic { get; set; }
    public DateTime? LastVisitDate { get; set; }
    public string? NameAbbreviation { get; set; }
}