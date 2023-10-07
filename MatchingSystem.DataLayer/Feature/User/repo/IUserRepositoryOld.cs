using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.DataLayer.Feature.User.repo
{
    public interface IUserRepositoryOld
    {
        
        IEnumerable<TutorInitDto> saveTutors(List<TutorInitDto> tutors);

        void AssignRoleForUser(int userId, int matchingId, int roleId);
       // List<StudentInitDto> saveStudents(List<StudentInitDto> users);
       // IEnumerable<RoleMatching> GetRolesForUser(int userId);
       // void UpdatePasswordHash(int userId, string newHash);
       // void SetLastVisitDate(int userId, string role, int projectId);
       public void ReadNotifications(int userId, int matchingId, int tutorId);
    }
}
