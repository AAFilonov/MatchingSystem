using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Interface
{
    public interface IUserRepository
    {
        IEnumerable<Role> GetRolesForUserAndMatching(int userId, int matchingId);
        OldEntities.User GetUser(string login);
        int GetUserIdByLogin(string login);
        string GetPasswordHashByLogin(string login);
        IEnumerable<TutorInitDto> SetUserIdForTutors(List<TutorInitDto> tutors);

        void AssignRoleForUser(int userId, int matchingId);
        List<StudentInitDto> CreateUsersForStudents(List<StudentInitDto> users);
        IEnumerable<RoleMatching> GetAllRoles(int userId);
        void UpdatePasswordHash(int userId, string newHash);
        void SetLastVisitDate(int userId, string role, int projectId);
        void ReadNotifications(int userId, int matchingId, int tutorId = default);
        IEnumerable<OldEntities.User> getStudentUsersByMatching(int matchingId);
    }
}
