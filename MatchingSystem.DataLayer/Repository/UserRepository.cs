using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Base;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;

namespace MatchingSystem.DataLayer.Repository
{
    public class UserRepository : ConnectionBase, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString)
        {
        }
        
        public static string Transliterate(string str)
        {
            string[] lat_up = {"A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh", "Ts", "Ch", "Sh", "Shch", "", "Y", "", "E", "Yu", "Ya"};
            string[] lat_low = {"a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "", "y", "", "e", "yu", "ya"};
            string[] rus_up = {"А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я"};
            string[] rus_low = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я"};
            for (int i = 0; i <= 32; i++)
            {
                str = str.Replace(rus_up[i],lat_up[i]);
                str = str.Replace(rus_low[i],lat_low[i]);              
            }
            return str;
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
        
        public  IEnumerable<TutorInitDto> SetUserIdForTutors(List<TutorInitDto> tutors)
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

        public  void AssignRoleForUser(int userId,int matchingId)
        {
            Connection.Execute(
                "insert into Users_Roles (UserID,RoleID,MatchingID) VALUES(@UserId,@RoleId,@MatchingId)"
                , new
                {
                    UserId = userId, RoleId = 3, MatchingId = matchingId
                }
            );
        }


        public List<StudentInitDto> CreateUsersForStudents(List<StudentInitDto> users)
        {
            foreach (var user in users)
            {
                user.UserId =Connection.ExecuteScalar<int>(
                    "insert into Users (Login,PasswordHash,Name,Surname,Patronimic) OUTPUT INSERTED.UserID Values(@Login,@Password,@Name,@Surname,@Patronimic)",
                    new
                    {
                        Name = user.firstName
                        ,Surname = user.lastName
                        ,Patronimic = user.middleName
                        ,Password = "$s2$16384$8$1$DJBTMOcK+VGXFk8BTUvWYNr7PZE4Cx0l2OdvbWA4/TA=$R7TZahOx+lmeP+B8FiLe6IQzQJ/mSVYQa+7M57kvcOs="//user.password
                        ,Login = Transliterate(user.lastName + user.firstName.Substring(0, 1) +
                                         ((user.middleName != null) ? user.middleName.Substring(0, 1) : ""))
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
