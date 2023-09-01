using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.User.service
{
    public interface IUserRepositoryOld
    {
 

        IEnumerable<TutorInitDto> saveTutors(List<TutorInitDto> tutors);

        //void AssignRoleForUser(int userId, int matchingId);
       // List<StudentInitDto> saveStudents(List<StudentInitDto> users);
       // IEnumerable<RoleMatching> GetRolesForUser(int userId);
       // void UpdatePasswordHash(int userId, string newHash);
       // void SetLastVisitDate(int userId, string role, int projectId);
       // void ReadNotifications(int userId, int matchingId, int tutorId = default);
        IEnumerable<UserDto> getStudentUsersByMatching(int matchingId);
    }
}
