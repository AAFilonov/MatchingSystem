using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.IO.Params;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Feature.User.service;
using MatchingSystem.DataLayer.OldEntities;
using MatchingSystem.DataLayer.Utils;
using Scrypt;

namespace MatchingSystem.DataLayer.Feature.Student;

public class StudentService : IStudentService
{
    private readonly IProjectRepository projectRepository;
    private readonly IStudentRepositoryOld studentRepositoryOld;
    private readonly IUserRepository userRepository;
    private readonly IStudentRepository studentRepository;

    public StudentService(
        IStudentRepositoryOld studentRepositoryOld,
        IProjectRepository projectRepository, IStudentRepository studentRepository, IUserRepository userRepository)
    {
        this.studentRepositoryOld = studentRepositoryOld;
        this.projectRepository = projectRepository;
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

    public IEnumerable<ProjectForStudent> GetProjects(int studentId)
    {
        var model = projectRepository.GetProjectsByStudent(studentId);

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
            User.User user = userRepository.findById(student.UserId);
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