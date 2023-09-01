using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Role.service;

public interface IRoleRepository:IRepository<Role>
{
    IEnumerable<RoleDto> GetRolesForUserAndMatching(int userId, int matchingId);
    IEnumerable<RoleMatching> GetRolesForUser(int userId);
}