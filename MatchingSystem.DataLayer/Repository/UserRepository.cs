using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Base;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;

namespace MatchingSystem.DataLayer.Repository
{
    public class UserRepository : ConnectionBase, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<RoleMatching> GetAllRoles(int userId)
        {
            return Connection.Query<RoleMatching>("select * from napp.get_UserRolesMatchings(@UserID, null)",
                new { UserID = userId });
        }

        public async Task<IEnumerable<RoleMatching>> GetAllRolesAsync(int userId)
        {
            return await Connection.QueryAsync<RoleMatching>("select * from napp.get_UserRolesMatchings(@UserID, null)",
                new { UserID = userId });
          
        }


        public string GetPasswordHashByLogin(string login)
        {
            return  Connection.QueryFirstOrDefault<string>("select napp.get_UserPasswordHash(@login)",
                new { Login = login });
        }

        public async Task<string> GetPasswordHashByLoginAsync(string login)
        {
            return await Connection.QueryFirstOrDefaultAsync<string>("select napp.get_UserPasswordHash(@login)",
                new { Login = login});
        }


        public IEnumerable<Role> GetRolesForUserAndMatching(int userId, int matchingId)
        {
            return Connection.Query<Role>("select * from napp.get_UserRoles_ByMatching (@UserID,@MatchingID)",
                new { UserID = userId, MatchingID= matchingId });
        }

        public async Task<IEnumerable<Role>> GetRolesForUserAndMatchingAsync(int userId, int matchingId)
        {
           return await Connection.QueryAsync<Role>("select * from napp.get_UserRoles_ByMatching (@UserID,@MatchingID)",
                new { UserID = userId, MatchingID = matchingId });
        }


        public User GetUser(string login)
        {
            return Connection.QueryFirstOrDefault<User>("select * from napp.get_User(null, @Login)", new { Login = login });
        }

        public async Task<User> GetUserAsync(string login)
        {
            return await Connection.QueryFirstOrDefaultAsync<User>("select * from  napp.get_User(null, @Login)", new { Login = login });
        }


        public int GetUserIdByLogin(string login)
        {
            return Connection.QueryFirstOrDefault<int>("select napp.get_UserID(@Login)", new { Login = login });
        }

        public async Task<int> GetUserIdByLoginAsync(string login)
        {
            return await Connection.QueryFirstOrDefaultAsync<int>("select napp.get_UserID(@Login)", new { Login = login });
        }


        public void ReadNotifications(int userId, int matchingId, int tutorId )
        {
            if (tutorId == 0)
            { //отвественный
                Connection.QueryFirstOrDefault(
                    "exec napp.upd_CommonQuota_Request_ReadNotifications @UserID, @MatchingID",
                    new {UserID = userId, MatchingID = matchingId});
            }
            else
            {
                //преподователь
                Connection.QueryFirstOrDefault(
                    "exec napp.upd_CommonQuota_Request_ReadNotifications @UserID, @MatchingID, @TutorID",
                    new {UserID = userId, MatchingID = matchingId, TutorID = tutorId});
            }
        }

        public async Task ReadNotificationsAsync(int userId, int matchingId, int tutorId )
        {

            await Connection.QueryFirstOrDefaultAsync("exec napp.upd_CommonQuota_Request_ReadNotifications @UserID, @MatchingID, @TutorID",
                new { UserID = userId, MatchingID = matchingId, TutorID = tutorId });
        }
        
        
        

        public void SetLastVisitDate(int userId, string role, int projectId)
        {
            Connection.QueryFirstOrDefault("exec napp.upd_User_LastVisitDate " +
               "@UserID, " +
               "null, " +
               "@RoleName, " +
               "@MatchingID",
               new { UserID = userId, RoleName = role, MatchingID = projectId });
           
        }

        public async Task SetLastVisitDateAsync(int userId, string role, int projectId)
        {
            await Connection.QueryFirstOrDefaultAsync("exec napp.upd_User_LastVisitDate " +
               "@UserID, " +
               "null, " +
               "@RoleName, " +
               "@MatchingID",
               new { UserID = userId, RoleName = role, MatchingID = projectId });
        }


        public void UpdatePasswordHash(int userId, string newHash)
        {
            Connection.QueryFirstOrDefault("exec napp.upd_User_PasswordHash " +
               "@UserID, " +
               "@NewPasswordHash",
               new { UserID = userId, NewPasswordHash = newHash });
        }

        public async Task UpdatePasswordHashAsync(int userId, string newHash)
        {
            await Connection.QueryFirstOrDefaultAsync("exec napp.upd_User_PasswordHash " +
               "@UserID, " +
               "@NewPasswordHash",
               new { UserID = userId, NewPasswordHash = newHash });
        }
    }
}
