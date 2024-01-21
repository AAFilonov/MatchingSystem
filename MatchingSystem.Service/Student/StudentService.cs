using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Project;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Student;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.Feature.User.repo;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.DataLayer.OldDto.IO.Params;
using MatchingSystem.DataLayer.OldDto.MatchingInit;
using MatchingSystem.DataLayer.OldEntities;
using MatchingSystem.DataLayer.Utils;
using Scrypt;

namespace MatchingSystem.Service.Student;

public class StudentService : IStudentService
{
    private readonly IProjectRepositoryOld projectRepositoryOld;
    private readonly IStudentRepositoryOld studentRepositoryOld;
    private readonly IUserRepository userRepository;
    private readonly IStudentRepository studentRepository;

    public StudentService(
        IStudentRepositoryOld studentRepositoryOld,
        IProjectRepositoryOld projectRepositoryOld, IStudentRepository studentRepository, IUserRepository userRepository)
    {
        this.studentRepositoryOld = studentRepositoryOld;
        this.projectRepositoryOld = projectRepositoryOld;
        this.studentRepository = studentRepository;
        this.userRepository = userRepository;
    }


    public StudentInfoDto getStudentInfo(int studentId)
    {
        var student = studentRepositoryOld.GetStudent(studentId);
        var model = new StudentInfoDto
        {
            Technologies = studentRepositoryOld.GetTechnologiesSelectedByStudent(studentId),
            WorkDirections = studentRepositoryOld.GetWorkDirectionsSelectedByStudent(studentId),
            Info = student.Info,
            Info2 = student.Info2
        };
        return model;
    }

    public void EditProfile(EditProfileParams editParams)
    {
        studentRepositoryOld.EditProfile(editParams);
    }

    public IEnumerable<ProjectForStudentDto> GetProjects(int studentId)
    {
        var model = projectRepositoryOld.GetProjectsByStudent(studentId);

        return model.OrderBy(x => x.OrderNumber);
    }

    public void SetPreferences(int studentId, string selectedList)
    {
        var temp = selectedList.Split(',');
        var selectedIds = new int[temp.Length];


        studentRepositoryOld.ClearPreferences(studentId);

        for (var i = 0; i < temp.Length; i++)
        {
            selectedIds[i] = Convert.ToInt32(temp[i]);
            studentRepositoryOld.SetPreferences(new StudentPreferenceParams
            {
                StudentId = studentId,
                Order = i + 1,
                SelectedProjectId = selectedIds[i]
            });
        }
    }

    public StudentDto GetStudentInfo(int? studentId)
    {
        var student = studentRepositoryOld.GetStudent(studentId.Value);

        return student;
    }

    public AllocatedByStudent GetAllocatedProject(int? studentId)
    {
        var model = studentRepositoryOld.GetAllocationByStudent(studentId.Value);

        return model;
    }

    public List<StudentInitDto> resetPasswords(List<StudentView> students)
    {
        List<StudentInitDto> dtos = new List<StudentInitDto>();
        foreach (var student in students)
        {
            var password = UserUtils.CreatePassword(8);
            var encoder = new ScryptEncoder();
            DataLayer.Feature.User.UserEntity user = userRepository.findById(student.UserId);
            user.PasswordHash = encoder.Encode(password);
            userRepository.update(user);
            dtos.Add(new StudentInitDto()
            {
                groupName = student.GroupName,
                firstName = student.Name,
                middleName = student.Patronimic,
                lastName = student.Surname,
                login = user.Login,
                password = password
            });
        }

        return dtos;

    }
}