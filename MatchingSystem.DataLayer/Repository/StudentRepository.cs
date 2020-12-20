using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Base;
using Microsoft.Data.SqlClient;

namespace MatchingSystem.DataLayer.Repository
{
    public class StudentRepository : ConnectionBase, IStudentRepository
    {
        public Task<AllocatedByStudent> GetAllocationByStudentAsync(int studentId)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> GetStudentAsync(int studentId)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);

            var result = await db.QueryFirstAsync<Student>(
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
            using IDbConnection db = new SqlConnection(ConnectionString);

            var result = db.QueryFirst<Student>(
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
            using IDbConnection db = new SqlConnection(ConnectionString);
            db.Open();
            var studentId = await db.ExecuteScalarAsync<int>(
                "select napp.get_StudentID(@UserID, @MatchingID) as Value", 
                new { UserId = userId, MatchingId = matchingId }
            );

            return studentId;
        }

        public int GetStudentId(int userId, int matchingId)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            
            var studentId = db.ExecuteScalar<int>(
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

        public IEnumerable<Technology> GetTechnologiesSelectedByStudent(int studentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkDirection>> GetWorkDirectionsSelectedByStudentAsync(int studentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WorkDirection> GetWorkDirectionsSelectedByStudent(int studentId)
        {
            throw new NotImplementedException();
        }

        public StudentRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
