using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Interface
{
    interface IUserRepository
    {
        //prev: GetRolesForUserAsync
        Task<IEnumerable<Role>> GetRolesForUserAndMatchingAsync(int userId, int matchingId);
        Task<User> GetUserAsync(string login);
        Task<User> GetUserAsync(string login, int userId);
        Task<int> GetUserIdByLoginAsync(string login);
        Task<string> GetPasswordHashByLoginAsync(string login);
        Task<RoleMatching> GetAllRolesAsync(int userId);
        Task UpdatePasswordHashAsync(int userId, string newHash);
        Task SetLastVisitDateAsync(int userId, string role, int projectId);
    }
}
