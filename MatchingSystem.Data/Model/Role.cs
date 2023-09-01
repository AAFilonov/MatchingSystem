using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Model;

public class Role
{
    public Role()
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