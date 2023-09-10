using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.IO.Params;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Student;

public interface IStudentService
{
    public StudentInfoDto getStudentInfo(int studentId);

    public void EditProfile(EditProfileParams editParams);

    public IEnumerable<ProjectForStudent> GetProjects(int studentId);

    public void SetPreferences(int studentId, string selectedList);

    public StudentDto GetStudentInfo(int? studentId);
    public AllocatedByStudent GetAllocatedProject(int? studentId);
    List<StudentInitDto> resetPasswords(List<StudentView> students);
}