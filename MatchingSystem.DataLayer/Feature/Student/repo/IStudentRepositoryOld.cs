using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Feature.Dictionary;
using MatchingSystem.DataLayer.Feature.StudentPreference;
using MatchingSystem.DataLayer.OldDto.IO.Params;
using MatchingSystem.DataLayer.OldDto.MatchingInit;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Student.repo
{
    public interface IStudentRepositoryOld
    {
        Task<AllocatedByStudent> GetAllocationByStudentAsync(int studentId);
        AllocatedByStudent GetAllocationByStudent(int studentId);
        Task<StudentDto> GetStudentAsync(int studentId);
        StudentDto GetStudent(int studentId);
        IEnumerable<StudentPreferenceDto> GetStudentPreferencesByMatching(int matchingId);
        public IEnumerable<StudentPreferenceDto> GetStudentAssignedToProject(int matchingId);
        IEnumerable<StudentPreferenceDto> GetStudentAvailablePreferencesByMatching(int matchingId);
        Task<int> GetStudentIdAsync(int userId, int matchingId);
        int GetStudentId(int userId, int matchingId);
        void CreateUserRoles_Students(List<StudentInitDto> studs, int matchingID);
        IEnumerable<StudentInitDto> CreateStudents(List<StudentInitDto> users,int matchingId);
        Task<IEnumerable<TechnologyDto>> GetTechnologiesSelectedByStudentAsync(int studentId);
        IEnumerable<TechnologyDto> GetTechnologiesSelectedByStudent(int studentId);
        Task<IEnumerable<WorkDirectionDto>> GetWorkDirectionsSelectedByStudentAsync(int studentId);
        IEnumerable<WorkDirectionDto> GetWorkDirectionsSelectedByStudent(int studentId);
        Task EditProfileAsync(EditProfileParams inParams);
        void EditProfile(EditProfileParams inParams);
        void SetPreferences(StudentPreferenceParams inParams);
        Task SetPreferencesAsync(StudentPreferenceParams inParams);
        void ClearPreferences(int studentId);
        Task ClearPreferencesAsync(int studentId);
        IEnumerable<StudentDto> GetStudentsByMatching(int matchingId);
    }
}
