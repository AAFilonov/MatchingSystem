using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.IO.Params;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Student;

public interface IStudentService
{
    public StudentInfoDto getStudentInfo(int studentId);

    public void EditProfile(EditProfileParams editParams);

    public IEnumerable<ProjectForStudentDto> GetProjects(int studentId);

    public void SetPreferences(int studentId, string selectedList);

    public StudentDto GetStudentInfo(int? studentId);
    public AllocatedByStudent GetAllocatedProject(int? studentId);
    List<StudentInitDto> resetPasswords(List<StudentView> students);
}