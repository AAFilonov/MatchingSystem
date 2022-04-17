using System;
using System.Collections.Generic;

namespace MatchingSystem.Data.Model;

public partial class Student1
{
    public int StudentId { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; } = null!;
    public int UserId { get; set; }
    public int? MatchingId { get; set; }
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Patronimic { get; set; }
    public string? NameAbbreviation { get; set; }
    public DateTime? LastVisitDate { get; set; }
    public string? Info { get; set; }
    public string? Info2 { get; set; }
    public string? WorkDirectionsNameList { get; set; }
    public string? TechnologiesNameList { get; set; }
}