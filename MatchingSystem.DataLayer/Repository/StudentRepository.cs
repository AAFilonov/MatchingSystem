using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Base;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.IO.Params;

namespace MatchingSystem.DataLayer.Repository
{
    public class StudentRepository : ConnectionBase, IStudentRepository
    {

        public StudentRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<AllocatedByStudent> GetAllocationByStudentAsync(int studentId)
        {
            var result = await Connection.QueryFirstOrDefaultAsync(
                    "select" +
                    " TutorNameAbbreviation," +
                    " ProjectName " +
                    "from napp.get_AllocatedProject_ByStudent(@StudentID)",
                    new { StudentId = studentId }
                );
            return result;
        }

        public AllocatedByStudent GetAllocationByStudent(int studentId)
        {
            var result =  Connection.QueryFirstOrDefault(
                    "select" +
                    " TutorNameAbbreviation," +
                    " ProjectName " +
                    "from napp.get_AllocatedProject_ByStudent(@StudentID)",
                    new { StudentId = studentId }
                );
            return result;
        }


        public async Task<Student> GetStudentAsync(int studentId)
        {
          var result = await Connection.QueryFirstAsync<Student>(
                "select " +
                "StudentID, " + 
                "GroupID, " +
                "GroupName, " +
                "Surname, " +
                "Name, " +
                "Patronimic, " +
                "Info, " +
                "Info2, " +
                "WorkDirectionsName_List, " +
                "TechnologiesName_List " +
                "from napp.get_StudentInfo(@StudentID)",
                new { StudentId = studentId }
            );
            return result;
        }

        public Student GetStudent(int studentId)
        {
            var result = Connection.QueryFirst<Student>(
                "select " +
                "StudentID, " + 
                "GroupID, " +
                "GroupName, " +
                "Surname, " +
                "Name, " +
                "Patronimic, " +
                "Info, " +
                "Info2, " +
                "WorkDirectionsName_List, " +
                "TechnologiesName_List " +
                "from napp.get_StudentInfo(@StudentID)",
                new { StudentId = studentId }
            );
            return result;
        }
        public IEnumerable<StudentPreferences> GetStudentPreferencesByMatching(int matchingId)
        {
            var result = Connection.Query<StudentPreferences>(
                "SELECT " +
                "stud.StudentID, " +
                "stud_pref.ProjectID, " +
                "stud_pref.OrderNumber " +
                "from dbo_v.Students stud " +
                "left join dbo.StudentsPreferences  stud_pref on " +
                "stud_pref.StudentID = stud.StudentID " +
                "WHERE MatchingID = @MatchingID",
                new { MatchingID = matchingId }
            );
            return result;
        }
        
        public IEnumerable<StudentPreferences> GetStudentAssignedToProject(int matchingId)
        {
            var result = Connection.Query<StudentPreferences>(
                "SELECT " +
                "stud.StudentID, " +
                "tch.ProjectID, " +
                "tch.SortOrderNumber " +
                "from dbo_v.Students stud " +
                "left join dbo.TutorsChoice  tch on " +
                "tch.StudentID = stud.StudentID " +
                "WHERE MatchingID = @MatchingID",
                new { MatchingID = matchingId }
            );
            return result;
        }

        public IEnumerable<StudentPreferences> GetStudentAvailablePreferencesByMatching(int matchingId)
        {
            var result = Connection.Query<StudentPreferences>(
                   "SELECT " +
                   "stud.StudentID," +
                   "stud_pref.ProjectID," +
                   "stud_pref.OrderNumber" +
                   "from dbo_v.Students stud " +
                   "left join dbo_v.AvailableStudentsPreferences  stud_pref on " +
                   "stud_pref.StudentID = stud.StudentID" +
                   "WHERE MatchingID = @MatchingID",
                   new { MatchingID = matchingId }
               );
            return result;
        }


        public Student GetStudents(int studentId)
        {
            var result = Connection.QueryFirst<Student>(
                "select " +
                "StudentID, " +
                "GroupID, " +
                "GroupName, " +
                "Surname, " +
                "Name, " +
                "Patronimic, " +
                "Info, " +
                "Info2, " +
                "WorkDirectionsName_List, " +
                "TechnologiesName_List " +
                "from napp.get_StudentInfo(@StudentID)",
                new { StudentId = studentId }
            );
            return result;
        }


        public async Task<int> GetStudentIdAsync(int userId, int matchingId)
        {

            var studentId = await Connection.ExecuteScalarAsync<int>(
                "select napp.get_StudentID(@UserID, @MatchingID) as Value", 
                new { UserId = userId, MatchingId = matchingId }
            );

            return studentId;
        }

        public int GetStudentId(int userId, int matchingId)
        {
             
            var studentId = Connection.ExecuteScalar<int>(
                "select napp.get_StudentID(@UserID, @MatchingID) as Value", 
                new { UserId = userId, MatchingId = matchingId }
            );

            return studentId;
        }

        public async Task<IEnumerable<Technology>> GetTechnologiesSelectedByStudentAsync(int studentId)
        {
            return await Connection.QueryAsync<Technology>(
                "select TechnologyCode, TechnologyName_ru from napp.get_Technologies_WithSelected_ByStudent(@StudentID) where IsSelectedByStudent = 1",
                studentId
            );
        }

        public void setNewUserRoles_Students(List<StudentInitDto> studs,int matchingID)
        {
            foreach (var stud in studs)
            {
                Connection.ExecuteAsync(
                    "insert Users_Roles (UserID,RoleID,MatchingID,StudentID) VALUES (@UserID,@RoleID,@MatchingID,@StudentID)", new
                    {
                        UserID = stud.UserId
                        ,RoleID = 2
                        ,MatchingID = matchingID
                        ,StudentID = stud.StudentId
                    });
            }
        }
        
        public IEnumerable<StudentInitDto> SetNewStudents(List<StudentInitDto> studs,int matchingId)
        {
            foreach (var stud in studs)
            {
                stud.StudentId = Connection.ExecuteScalar<int>(
                    "insert Students (GroupID,MatchingID) OUTPUT INSERTED.StudentId VALUES (@GroupID,@MatchingID)", new
                    {
                        GroupID = stud.GroupId,
                        MatchingID = matchingId
                    });
            }

            return studs;
        }

        public IEnumerable<Technology> GetTechnologiesSelectedByStudent(int studentId)
        {
            return Connection.Query<Technology>(
                  "select TechnologyCode, TechnologyName_ru from napp.get_Technologies_WithSelected_ByStudent(@StudentID) where IsSelectedByStudent = 1",
                  new
                  {
                      StudentID = studentId}
                      );
        }

        public async Task<IEnumerable<WorkDirection>> GetWorkDirectionsSelectedByStudentAsync(int studentId)
        {
         
            return await Connection.QueryAsync<WorkDirection>(
                "select DirectionCode, DirectionName_ru from napp.get_WorkDirections_WithSelected_ByStudent(@StudentID) where IsSelectedByStudent = 1",
                new
                {
                    StudentID = studentId
                }
                
            );
        }

        public IEnumerable<WorkDirection> GetWorkDirectionsSelectedByStudent(int studentId)
        {
            return  Connection.Query<WorkDirection>(
               "select DirectionCode, DirectionName_ru from napp.get_WorkDirections_WithSelected_ByStudent(@StudentID) where IsSelectedByStudent = 1",
               new
               {
                   StudentID = studentId
               }
               
           );
        }

        public async Task EditProfileAsync(EditProfileParams inParams)
        {
            await Connection.ExecuteAsync(
                "exec napp.upd_Student_Info @StudentID, @Info, @Info2, @Technology_CodeList, @WorkDirection_CodeList",
                new
                {
                    StudentID = inParams.StudentId,
                    Info = inParams.Info,
                    Info2 = inParams.Info2,
                    Technology_CodeList = inParams.TechnologyCodeList,
                    WorkDirection_CodeList = inParams.WorkDirectionCodeList
                }
            );
        }

        public void EditProfile(EditProfileParams inParams)
        {
            Connection.Execute(
                "exec napp.upd_Student_Info @StudentID, @Info, @Info2, @Technology_CodeList, @WorkDirection_CodeList",
                new
                {
                    StudentID = inParams.StudentId,
                    Info = inParams.Info,
                    Info2 = inParams.Info2,
                    Technology_CodeList = inParams.TechnologyCodeList,
                    WorkDirection_CodeList = inParams.WorkDirectionCodeList
                }
            );
        }

        public void SetPreferences(StudentPreferenceParams inParams)
        {
            Connection.Execute(
                "exec napp.create_StudentsPreference @StudentID, @ProjectID, @OrderNumber",
                new
                {
                    StudentID = inParams.StudentId,
                    ProjectID = inParams.SelectedProjectId,
                    OrderNumber = inParams.Order
                }
            );
        }

        public async Task SetPreferencesAsync(StudentPreferenceParams inParams)
        {
            await Connection.ExecuteAsync(
                "exec napp.create_StudentsPreference @StudentID, @ProjectID, @OrderNumber",
                new
                {
                    StudentID = inParams.StudentId,
                    ProjectID = inParams.SelectedProjectId,
                    OrderNumber = inParams.Order
                }
            );
        }

        public void ClearPreferences(int studentId)
        {
            Connection.Execute(
                "exec napp.del_StudentsPreferences @StudentID", 
                new {StudentID = studentId}
            );
        }

        public async Task ClearPreferencesAsync(int studentId)
        {
            await Connection.ExecuteAsync(
                "exec napp.del_StudentsPreferences @StudentID", 
                new {StudentID = studentId}
            );
        }
    }
}
