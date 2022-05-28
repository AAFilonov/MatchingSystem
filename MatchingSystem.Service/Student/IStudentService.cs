using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Dto;

namespace Service.Student;

public interface IStudentService
{
    public GetData GetSelectedParams(int studentId);

    public void EditProfile(int studentId, string info, string info2, string tech, string workDirection);

    public IEnumerable<ProjectForStudent> GetProjects(int studentId);

    public void SetPreferences(int studentId, string selectedList);

    public MatchingSystem.DataLayer.Entities.Student GetStudentInfo(int? studentId);
    public AllocatedByStudent GetAllocatedProject(int? studentId);
}