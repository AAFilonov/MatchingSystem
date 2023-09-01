using System;

namespace MatchingSystem.DataLayer.Model;

public class UsersFullInfo
{
    public int UserId { get; set; }
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Patronimic { get; set; }
    public DateTime? LastVisitDate { get; set; }
    public int? MatchingId { get; set; }
    public string? NameAbbreviation { get; set; }
    public int? StudentId { get; set; }
    public int? TutorId { get; set; }
    public int? RoleId { get; set; }
    public int? RoleCode { get; set; }
    public short? RoleType { get; set; }
    public string? RoleName { get; set; }
    public string? RoleNameRu { get; set; }
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }
}