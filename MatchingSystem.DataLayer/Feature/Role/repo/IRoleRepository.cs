using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldDto.MatchingInit;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Role.repo;

public interface IRoleRepository:IRepository<UsersRoleEntity>
{
    IEnumerable<RoleDto> GetRolesForUserAndMatching(int userId, int matchingId);
    IEnumerable<RoleDto> GetRoles();
    IEnumerable<RoleMatching> GetRolesForUser(int userId);
    void AssignRoleForUser(int userId, int matchingId, int roleId, DiplomaMatchingContext? extContext = null);

    void AssignTutorRoles(List<TutorInitDto> dataTutorRecords, int matchingId, DiplomaMatchingContext? extContext = null);
    void AssignStudentsRoles(List<StudentInitDto> studentRecords, int matchingMatchingId,
        DiplomaMatchingContext? context = null);
}