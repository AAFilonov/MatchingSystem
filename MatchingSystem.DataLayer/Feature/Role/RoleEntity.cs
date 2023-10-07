using System.Collections.Generic;
using MatchingSystem.DataLayer.Model;

namespace MatchingSystem.DataLayer.Feature.Role;

public class RoleEntity
{
    public RoleEntity()
    {
        UsersRoles = new HashSet<UsersRole>();
    }

    public int RoleId { get; set; }
    public int? RoleCode { get; set; }
    public string? RoleName { get; set; }
    public short? RoleType { get; set; }
    public string? RoleNameRu { get; set; }

    public virtual ICollection<UsersRole> UsersRoles { get; set; }
}