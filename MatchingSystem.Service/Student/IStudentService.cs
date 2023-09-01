using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.IO.Params;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Student;

public interface IStudentService
{
    public StudentInfoDto getStudentInfo(int studentId);

    public void EditProfile(EditProfileParams editParams);

    public IEnumerable<ProjectForStudent> GetProjects(int studentId);

    public void SetPreferences(int studentId, string selectedList);

    public DataLayer.OldEntities.Student GetStudentInfo(int? studentId);
    public AllocatedByStudent GetAllocatedProject(int? studentId);
}