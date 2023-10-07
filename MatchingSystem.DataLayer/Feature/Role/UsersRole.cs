using System;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Role;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.User;

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

    public virtual MatchingEntity? Matching { get; set; }
    public virtual RoleEntity Role { get; set; } = null!;
    public virtual StudentEntity? Student { get; set; }
    public virtual Tutor? Tutor { get; set; }
    public virtual UserEntity User { get; set; } = null!;
}