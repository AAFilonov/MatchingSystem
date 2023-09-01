using System;
using System.Collections.Generic;
using MatchingSystem.DataLayer.Model;

// ReSharper disable CheckNamespace
namespace MatchingSystem.Data.Model;

public class User
{
    public User()
    {
        UsersRoles = new HashSet<UsersRole>();
    }

    public int UserId { get; set; }
    public string Login { get; set; } = null!;
    public string? PasswordHash { get; set; }
    public DateTime? LastVisitDate { get; set; }
    public int? UserBk { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Patronimic { get; set; }
    public string? NameAbbreviation
    {
        get;
    }

    public virtual IEnumerable<UsersRole> UsersRoles { get; set; }

    public override string ToString()
    {
        return $"{nameof(UserId)}: {UserId}, {nameof(Login)}: {Login},{nameof(LastVisitDate)}: {LastVisitDate}, {nameof(UserBk)}: {UserBk}, {nameof(Email)}: {Email}, {nameof(Name)}: {Name}, {nameof(Surname)}: {Surname}, {nameof(Patronimic)}: {Patronimic}, {nameof(UsersRoles)}: {UsersRoles}";
    }
}