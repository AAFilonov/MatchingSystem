using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.IO.Params;

namespace MatchingSystem.Service.Student;

public interface IStudentService
{
    public GetData GetSelectedParams(int studentId);

    public void EditProfile(EditProfileParams editParams);

    public IEnumerable<ProjectForStudent> GetProjects(int studentId);

    public void SetPreferences(int studentId, string selectedList);

    public MatchingSystem.DataLayer.Entities.Student GetStudentInfo(int? studentId);
    public AllocatedByStudent GetAllocatedProject(int? studentId);
}