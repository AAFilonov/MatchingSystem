using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Role.repo;

public interface IRoleRepository:IRepository<UsersRoleEntity>
{
    IEnumerable<RoleDto> GetRolesForUserAndMatching(int userId, int matchingId);
    IEnumerable<RoleMatching> GetRolesForUser(int userId);
    void AssignRoleForUser(int userId, int matchingId, int roleId);
    void AssignTutorRole(List<TutorInitDto> dataTutorRecords, int matchingId);
}