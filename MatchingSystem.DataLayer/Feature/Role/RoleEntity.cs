using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Feature.Role;

public class RoleEntity
{
    public RoleEntity()
    {
        UsersRoles = new HashSet<UsersRoleEntity>();
    }

    public int RoleId { get; set; }
    public int? RoleCode { get; set; }
    public string? RoleName { get; set; }
    public short? RoleType { get; set; }
    public string? RoleNameRu { get; set; }

    public virtual ICollection<UsersRoleEntity> UsersRoles { get; set; }
}