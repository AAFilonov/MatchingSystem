using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Context;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.User.service
{
    public class UserRepositoryOld : ConnectionBase, IUserRepositoryOld
    {
        public UserRepositoryOld(string connectionString) : base(connectionString)
        {
        }
        
      

        public IEnumerable<RoleMatching> GetRolesForUser(int userId)
        {
            return Connection.Query<RoleMatching>("select * from napp.get_UserRolesMatchings(@UserID, null)",
                new { UserID = userId });
        }

        public async Task<IEnumerable<RoleMatching>> GetAllRolesAsync(int userId)
        {
            return await Connection.QueryAsync<RoleMatching>("select * from napp.get_UserRolesMatchings(@UserID, null)",
                new { UserID = userId });
          
        }
        
        public  IEnumerable<TutorInitDto> saveTutors(List<TutorInitDto> tutors)
        {
            foreach (var tutor in tutors)
            {
                tutor.UserId = Connection.ExecuteScalar<int>(
                    "Select Top 1 UserID from Users where Surname+' '+Name+' '+ Patronimic=@NameAbbriviation"
                    , new { NameAbbriviation = tutor.nameAbbreviation }
                );
                if (tutor.UserId == 0)
                {
                    tutor.UserId = Connection.ExecuteScalar<int>(
                        "Select top 1 UserID from Users where Surname+' '+left(Name,1)+'. '+ left(Patronimic,1)+'.'=@NameAbbriviation"
                        , new { NameAbbriviation = tutor.nameAbbreviation }
                    );  
                }
            }
            return tutors;

        }

        public void AssignRoleForUser(int userId, int matchingId, int roleId)
        {
            Connection.Execute(
                "insert into Users_Roles (UserID,RoleID,MatchingID) VALUES(@UserId,@RoleId,@MatchingId)"
                , new
                {
                    UserId = userId, RoleId = roleId, MatchingId = matchingId
                }
            );
        }


        public List<StudentInitDto> saveStudents(List<StudentInitDto> users)
        {
            foreach (var user in users)
            {
                user.UserId =Connection.ExecuteScalar<int>(
                    "insert into Users (Login,PasswordHash,Name,Surname,Patronimic, UserBK) OUTPUT INSERTED.UserID Values(@Login,@Password,@Name,@Surname,@Patronimic,@UserBK)",
                    new
                    {
                        Name = user.firstName
                        ,Surname = user.lastName
                        ,Patronimic = user.middleName
                        ,Password = user.passwordHash
                        ,Login = user.login
                        ,UserBK = user.userBK
                        
                    });
            }
            foreach (var user in users)
            {
                user.UserId =Connection.ExecuteScalar<int>(
                    "update Users set UserBK = userBK where UserID = @UserId",
                    new
                    { 
                        UserID = user.UserId,
                        UserBK = user.userBK
                    });
            }
            
            return users;
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


        public IEnumerable<RoleDto> GetRolesForUserAndMatching(int userId, int matchingId)
        {
            return Connection.Query<RoleDto>("select * from napp.get_UserRoles_ByMatching (@UserID,@MatchingID)",
                new { UserID = userId, MatchingID= matchingId });
        }

        public async Task<IEnumerable<RoleDto>> GetRolesForUserAndMatchingAsync(int userId, int matchingId)
        {
           return await Connection.QueryAsync<RoleDto>("select * from napp.get_UserRoles_ByMatching (@UserID,@MatchingID)",
                new { UserID = userId, MatchingID = matchingId });
        }


        public UserDto GetUser(string login)
        {
            return Connection.QueryFirstOrDefault<UserDto>("select * from napp.get_User(null, @Login)", new { Login = login });
        }

        public async Task<UserDto> GetUserAsync(string login)
        {
            return await Connection.QueryFirstOrDefaultAsync<UserDto>("select * from  napp.get_User(null, @Login)", new { Login = login });
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
