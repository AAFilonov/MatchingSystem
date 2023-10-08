using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.DataLayer.Feature.User.repo;

public interface IUserRepository :IRepository<UserEntity> 
{
    UserEntity? GetByLogin(string authLogin);
    IEnumerable<TutorInitDto> SetUserIdForTutors(List<TutorInitDto> tutors);
    
    IEnumerable<StudentInitDto> SaveStudents(List<StudentInitDto> users);
    IEnumerable<UserDto> GetStudentUsersByMatching(int matchingId);
    
    void UpdatePasswordHash(int userId, string newHash);
    public void SetLastVisitDate(int userId, string roleId, int projectId);
}