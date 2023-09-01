using System;
using MatchingSystem.Data.Model;

namespace MatchingSystem.DataLayer.Model;
public class UsersRole
{
    public int UserRoleId { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public int? MatchingId { get; set; }
    public DateTime? LastVisitDate { get; set; }
    public int? StudentId { get; set; }
    public int? TutorId { get; set; }

    public virtual Matching? Matching { get; set; }
    public virtual Role Role { get; set; } = null!;
    public virtual Student? Student { get; set; }
    public virtual Tutor? Tutor { get; set; }
    public virtual User User { get; set; } = null!;
}