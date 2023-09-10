using System.Collections.Generic;
using MatchingSystem.DataLayer.Model;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Role.service;

public interface IRoleRepository:IRepository<UsersRole>
{
    IEnumerable<RoleDto> GetRolesForUserAndMatching(int userId, int matchingId);
    IEnumerable<RoleMatching> GetRolesForUser(int userId);
    void AssignRoleForUser(int userId, int matchingId, int roleId);
}