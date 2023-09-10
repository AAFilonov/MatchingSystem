using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto.IO.Params;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Student
{
    public interface IStudentRepositoryOld
    {
        Task<AllocatedByStudent> GetAllocationByStudentAsync(int studentId);
        AllocatedByStudent GetAllocationByStudent(int studentId);
        Task<StudentDto> GetStudentAsync(int studentId);
        StudentDto GetStudent(int studentId);
        IEnumerable<StudentPreferences> GetStudentPreferencesByMatching(int matchingId);
        public IEnumerable<StudentPreferences> GetStudentAssignedToProject(int matchingId);
        IEnumerable<StudentPreferences> GetStudentAvailablePreferencesByMatching(int matchingId);
        Task<int> GetStudentIdAsync(int userId, int matchingId);
        int GetStudentId(int userId, int matchingId);
        void CreateUserRoles_Students(List<StudentInitDto> studs, int matchingID);
        IEnumerable<StudentInitDto> CreateStudents(List<StudentInitDto> users,int matchingId);
        Task<IEnumerable<Technology>> GetTechnologiesSelectedByStudentAsync(int studentId);
        IEnumerable<Technology> GetTechnologiesSelectedByStudent(int studentId);
        Task<IEnumerable<WorkDirection>> GetWorkDirectionsSelectedByStudentAsync(int studentId);
        IEnumerable<WorkDirection> GetWorkDirectionsSelectedByStudent(int studentId);
        Task EditProfileAsync(EditProfileParams inParams);
        void EditProfile(EditProfileParams inParams);
        void SetPreferences(StudentPreferenceParams inParams);
        Task SetPreferencesAsync(StudentPreferenceParams inParams);
        void ClearPreferences(int studentId);
        Task ClearPreferencesAsync(int studentId);
        IEnumerable<StudentDto> GetStudentsByMatching(int matchingId);
    }
}
