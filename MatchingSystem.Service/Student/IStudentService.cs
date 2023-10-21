using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.DataLayer.OldDto.IO.Params;
using MatchingSystem.DataLayer.OldDto.MatchingInit;
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