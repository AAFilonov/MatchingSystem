using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Interface
{
    public interface IStudentRepository
    {
        Task<AllocatedByStudent> GetAllocationByStudentAsync(int studentId);
        Task<Student> GetStudentAsync(int studentId);
        Student GetStudent(int studentId);
        Task<int> GetStudentIdAsync(int userId, int matchingId);
        int GetStudentId(int userId, int matchingId);
        Task<IEnumerable<Technology>> GetTechnologiesSelectedByStudentAsync(int studentId);
        IEnumerable<Technology> GetTechnologiesSelectedByStudent(int studentId);
        Task<IEnumerable<WorkDirection>> GetWorkDirectionsSelectedByStudentAsync(int studentId);
        IEnumerable<WorkDirection> GetWorkDirectionsSelectedByStudent(int studentId);
    }
}
