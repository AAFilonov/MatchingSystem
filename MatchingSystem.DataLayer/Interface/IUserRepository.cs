using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<Role>> GetRolesForUserAndMatchingAsync(int userId, int matchingId);
        IEnumerable<Role> GetRolesForUserAndMatching(int userId, int matchingId);
       User GetUser(string login);
        Task<User> GetUserAsync(string login);
        Task<int> GetUserIdByLoginAsync(string login);
        int GetUserIdByLogin(string login);
        Task<string> GetPasswordHashByLoginAsync(string login);
        string GetPasswordHashByLogin(string login);
        Task<IEnumerable<RoleMatching>> GetAllRolesAsync(int userId);
        IEnumerable<RoleMatching> GetAllRoles(int userId);
        Task UpdatePasswordHashAsync(int userId, string newHash);
        void UpdatePasswordHash(int userId, string newHash);
        Task SetLastVisitDateAsync(int userId, string role, int projectId);
        void SetLastVisitDate(int userId, string role, int projectId);
        Task ReadNotificationsAsync(int userId, int matchingId, int tutorId = default);
        void ReadNotifications(int userId, int matchingId, int tutorId = default);
    }
}
